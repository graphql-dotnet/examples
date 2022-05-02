// <copyright file="GraphQLExecuterExtensions.cs">
// MIT License, taken from https://github.com/tpeczek/Demo.Azure.Functions.GraphQL/blob/master/Demo.Azure.Functions.GraphQL/Infrastructure/GraphQLExecutionResult.cs
// </copyright>
// <author>https://github.com/tpeczek</author>
using GraphQL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Example
{
    internal class GraphQLExecutionResult : ActionResult
    {
        private const string CONTENT_TYPE = "application/json";

        private readonly ExecutionResult _executionResult;

        public GraphQLExecutionResult(ExecutionResult executionResult)
        {
            _executionResult = executionResult ?? throw new ArgumentNullException(nameof(executionResult));
        }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            IGraphQLSerializer documentSerializer = context.HttpContext.RequestServices.GetRequiredService<IGraphQLSerializer>();

            HttpResponse response = context.HttpContext.Response;
            response.ContentType = CONTENT_TYPE;
            response.StatusCode = StatusCodes.Status200OK;

            // Azure functions 3 disallowing async IO and newtonsoft json is not able to
            // make real async IO, we need copy to a MemoryStream.
            // After graphql has switch to System.Text.Json this can be written directly to response.Body
            await documentSerializer.WriteAsync(response.Body, _executionResult);
        }
    }
}
