using GraphQL;
using GraphQL.Server;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Example
{
    public class StarWars
    {
        private readonly IGraphQLExecuter<ISchema> graphQLExecuter;

        public StarWars(IGraphQLExecuter<ISchema> graphQLExecuter)
        {
            this.graphQLExecuter = graphQLExecuter;
        }

        [FunctionName("graphql")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                ExecutionResult executionResult = await graphQLExecuter.ExecuteAsync(req, log);

                if (executionResult.Errors != null)
                {
                    log.LogError("GraphQL execution error(s): {Errors}", executionResult.Errors);
                }

                return new GraphQLExecutionResult(executionResult);
            }
            catch (GraphQLBadRequestException ex)
            {
                return new BadRequestObjectResult(new { message = ex.Message });
            }
        }
    }
}
