using GraphQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Threading.Tasks;

namespace Example
{
    public class ExecutionResultActionResult : IActionResult
    {
        private readonly ExecutionResult _executionResult;

        public ExecutionResultActionResult(ExecutionResult executionResult)
        {
            _executionResult = executionResult;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var serializer = context.HttpContext.RequestServices.GetRequiredService<IGraphQLSerializer>();
            var response = context.HttpContext.Response;
            response.ContentType = "application/json";
            response.StatusCode = _executionResult.Data == null && !_executionResult.Executed ? (int)HttpStatusCode.BadRequest : (int)HttpStatusCode.OK;
            await serializer.WriteAsync(response.Body, _executionResult, context.HttpContext.RequestAborted);
        }
    }
}
