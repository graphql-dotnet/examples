using GraphQL.Types;
using GraphQL.Validation.Complexity;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace GraphQL.GraphiQL.Controllers
{
    public class GraphQLController : ApiController
    {
        private readonly IDocumentExecuter _executer;
        private readonly IGraphQLSerializer _serializer;

        public GraphQLController(
            IDocumentExecuter<ISchema> executer,
            IGraphQLSerializer serializer)
        {
            _executer = executer;
            _serializer = serializer;
        }

        // This will display an example error
        [HttpGet]
        public Task<HttpResponseMessage> GetAsync(HttpRequestMessage request)
        {
            return PostAsync(request, new GraphQLQuery { Query = "query foo { hero { id name appearsIn } }", Variables = null });
        }

        [HttpPost]
        public async Task<HttpResponseMessage> PostAsync(HttpRequestMessage request, GraphQLQuery query)
        {
            var variables = _serializer.ReadNode<Inputs>(query.Variables);
            var queryToExecute = query.Query;

            var result = await _executer.ExecuteAsync(_ =>
            {
                _.Query = queryToExecute;
                _.OperationName = query.OperationName;
                _.Variables = variables;

                _.ComplexityConfiguration = new ComplexityConfiguration { MaxDepth = 15 };

            }).ConfigureAwait(false);

            var httpResult = result.Executed
                ? HttpStatusCode.OK
                : HttpStatusCode.BadRequest;

            var response = request.CreateResponse(httpResult);
            response.Content = new PushStreamContent(
                (outputStream, httpContent, transportContext) => _serializer.WriteAsync(outputStream, result),
                "application/graphql+json");

            return response;
        }
    }

    public class GraphQLQuery
    {
        public string OperationName { get; set; }
        public string Query { get; set; }
        public Newtonsoft.Json.Linq.JObject Variables { get; set; }
    }
}
