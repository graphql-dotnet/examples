using GraphQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
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
            var writer = context.HttpContext.RequestServices.GetRequiredService<IDocumentWriter>();
            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = _executionResult.Errors?.Any() == true ? (int)HttpStatusCode.BadRequest : (int)HttpStatusCode.OK;
            await writer.WriteAsync(context.HttpContext.Response.Body, _executionResult, context.HttpContext.RequestAborted);
        }
    }
}
