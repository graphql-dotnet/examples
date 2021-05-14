using GraphQL;
using System.Text.Json.Serialization;

namespace Example
{
    public class GraphQLRequest
    {
        [JsonPropertyName("operationName")]
        public string OperationName { get; set; }

        [JsonPropertyName("query")]
        public string Query { get; set; }

        [JsonPropertyName("variables")]
        public GraphQL.Inputs Variables { get; set; }
    }
}
