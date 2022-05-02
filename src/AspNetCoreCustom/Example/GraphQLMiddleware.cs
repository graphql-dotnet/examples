using GraphQL;
using GraphQL.Instrumentation;
using GraphQL.SystemTextJson;
using GraphQL.Transport;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Example
{
    public class GraphQLMiddleware : IMiddleware
    {
        private readonly GraphQLSettings _settings;
        private readonly IDocumentExecuter<ISchema> _executer;
        private readonly IGraphQLSerializer _serializer;

        public GraphQLMiddleware(
            GraphQLSettings settings,
            IDocumentExecuter<ISchema> executer,
            IGraphQLSerializer serializer)
        {
            _settings = settings;
            _executer = executer;
            _serializer = serializer;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (!IsGraphQLRequest(context))
            {
                await next(context);
                return;
            }

            await ExecuteAsync(context);
        }

        private bool IsGraphQLRequest(HttpContext context)
        {
            return context.Request.Path.StartsWithSegments(_settings.Path)
                && string.Equals(context.Request.Method, "POST", StringComparison.OrdinalIgnoreCase);
        }

        private async Task ExecuteAsync(HttpContext context)
        {
            var request = await _serializer.ReadAsync<GraphQLRequest>(context.Request.Body, context.RequestAborted);

            var start = DateTime.UtcNow;

            var result = await _executer.ExecuteAsync(_ =>
            {
                _.Query = request?.Query;
                _.OperationName = request?.OperationName;
                _.Variables = request?.Variables;
                _.UserContext = _settings.BuildUserContext?.Invoke(context);
                _.EnableMetrics = _settings.EnableMetrics;
                _.RequestServices = context.RequestServices;
                _.CancellationToken = context.RequestAborted;
            });

            if (_settings.EnableMetrics)
            {
                result.EnrichWithApolloTracing(start);
            }

            await WriteResponseAsync(context, result);
        }

        private async Task WriteResponseAsync(HttpContext context, ExecutionResult result)
        {
            context.Response.ContentType = "application/graphql+json";
            context.Response.StatusCode = result.Executed ? (int)HttpStatusCode.OK : (int)HttpStatusCode.BadRequest;

            await _serializer.WriteAsync(context.Response.Body, result, context.RequestAborted);
        }
    }
}
