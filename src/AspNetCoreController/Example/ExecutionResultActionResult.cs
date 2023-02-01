using System.Net;
using System.Threading.Tasks;
using GraphQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

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
            response.StatusCode = _executionResult.Executed ? (int)HttpStatusCode.OK : (int)HttpStatusCode.BadRequest;
            await serializer.WriteAsync(response.Body, _executionResult, context.HttpContext.RequestAborted);
        }
    }
}
