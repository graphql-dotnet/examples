// <copyright file="GraphQLExecuterExtensions.cs">
// MIT License, taken from https://github.com/tpeczek/Demo.Azure.Functions.GraphQL/blob/master/Demo.Azure.Functions.GraphQL/Infrastructure/GraphQLExecuterExtensions.cs
// </copyright>
// <author>https://github.com/tpeczek</author>
using GraphQL;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Example
{
    internal static class GraphQLExecuterExtensions
    {
        private const string OPERATION_NAME_KEY = "operationName";
        private const string QUERY_KEY = "query";
        private const string VARIABLES_KEY = "variables";

        private const string JSON_MEDIA_TYPE = "application/json";
        private const string GRAPHQL_MEDIA_TYPE = "application/graphql";
        private const string FORM_URLENCODED_MEDIA_TYPE = "application/x-www-form-urlencoded";

        public async static Task<ExecutionResult> ExecuteAsync(this IDocumentExecuter documentExecuter, HttpRequest request, ILogger logger)
        {
            string? operationName = null;
            string query;
            Inputs variables;

            if (HttpMethods.IsGet(request.Method) || (HttpMethods.IsPost(request.Method) && request.Query.ContainsKey(QUERY_KEY)))
            {
                (operationName, query, variables) = ExtractGraphQLAttributesFromQueryString(request);
            }
            else if (HttpMethods.IsPost(request.Method))
            {
                if (!MediaTypeHeaderValue.TryParse(request.ContentType, out var mediaTypeHeader))
                {
                    throw new GraphQLBadRequestException($"Could not parse 'Content-Type' header value '{request.ContentType}'.");
                }

                switch (mediaTypeHeader.MediaType)
                {
                    case JSON_MEDIA_TYPE:
                        (operationName, query, variables) = await ExtractGraphQLAttributesFromJsonBodyAsync(request);
                        break;
                    case GRAPHQL_MEDIA_TYPE:
                        query = await ExtractGraphQLQueryFromGraphQLBodyAsync(request.Body);
                        variables = Inputs.Empty;
                        break;
                    case FORM_URLENCODED_MEDIA_TYPE:
                        (operationName, query, variables) = await ExtractGraphQLAttributesFromFormCollectionAsync(request);
                        break;
                    default:
                        throw new GraphQLBadRequestException($"Not supported 'Content-Type' header value '{request.ContentType}'.");
                }
            }
            else
            {
                throw new GraphQLBadRequestException($"Not supported 'HttpMethod' header value '{request.Method}'.");
            }

            logger.LogDebug("got graphql query: {operationName}, {query}, {variables}", operationName, query, variables);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var executonResult = await documentExecuter.ExecuteAsync(new ExecutionOptions
            {
                Query = query,
                OperationName = operationName,
                Variables = variables,
                RequestServices = request.HttpContext.RequestServices,
                CancellationToken = request.HttpContext.RequestAborted,
            });
            stopwatch.Stop();
            logger.LogMetric($"graphql.{operationName}", stopwatch.ElapsedMilliseconds);
            return executonResult;
        }

        private static (string? operationName, string query, Inputs variables) ExtractGraphQLAttributesFromQueryString(HttpRequest request)
        {
            return (
                request.Query.TryGetValue(OPERATION_NAME_KEY, out var operationNameValues) ? operationNameValues[0] : null,
                request.Query[QUERY_KEY][0],
                request.Query.TryGetValue(VARIABLES_KEY, out var variablesValues) ? variablesValues[0].ToInputs() : Inputs.Empty
            );
        }

        private async static Task<(string? operationName, string query, Inputs variables)> ExtractGraphQLAttributesFromJsonBodyAsync(HttpRequest request)
        {
            using StreamReader bodyReader = new StreamReader(request.Body);
            using JsonTextReader bodyJsonReader = new JsonTextReader(bodyReader);
            JObject bodyJson = await JObject.LoadAsync(bodyJsonReader);

            return (
                bodyJson.Value<string>(OPERATION_NAME_KEY),
                bodyJson.Value<string>(QUERY_KEY),
                bodyJson.Value<JObject>(VARIABLES_KEY).ToInputs()
            );
        }

        private static Task<string> ExtractGraphQLQueryFromGraphQLBodyAsync(Stream body)
        {
            using StreamReader bodyReader = new StreamReader(body);
            return bodyReader.ReadToEndAsync();
        }

        private async static Task<(string? operationName, string query, Inputs variables)> ExtractGraphQLAttributesFromFormCollectionAsync(HttpRequest request)
        {
            IFormCollection requestFormCollection = await request.ReadFormAsync();

            return (
                requestFormCollection.TryGetValue(OPERATION_NAME_KEY, out var operationNameValues) ? operationNameValues[0] : null,
                requestFormCollection[QUERY_KEY][0],
                requestFormCollection[VARIABLES_KEY][0].ToInputs()
                );
        }
    }
}
