using GraphQL;

namespace Example
{
    [GraphQLMetadata("Query")]
    public class QueryType
    {
        public string Hello()
        {
            return "World";
        }
    }
}
