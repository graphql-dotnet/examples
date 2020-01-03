// <copyright file="GraphQLExecuterExtensions.cs">
// MIT License, taken from https://github.com/tpeczek/Demo.Azure.Functions.GraphQL/blob/master/Demo.Azure.Functions.GraphQL/Infrastructure/GraphQLExecutionResult.cs
// </copyright>
// <author>https://github.com/tpeczek</author>
using GraphQL;
using GraphQL.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
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

            IDocumentWriter documentWriter = context.HttpContext.RequestServices.GetRequiredService<IDocumentWriter>();

            HttpResponse response = context.HttpContext.Response;
            response.ContentType = CONTENT_TYPE;
            response.StatusCode = StatusCodes.Status200OK;

            using var stream = new MemoryStream();
            await documentWriter.WriteAsync(stream, _executionResult);
            stream.Position = 0;
            await stream.CopyToAsync(response.Body);
        }
    }
}
