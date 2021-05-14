using System;
using GraphQL;

namespace Example
{
    public class GraphTypeMetadataAttribute : Attribute
    {
        public GraphTypeMetadataAttribute(string typeDef)
        {
            TypeDef = typeDef;
        }

        public string TypeDef { get; }
    }

    [GraphTypeMetadata(@"
        type User {
            id: ID
            name: String
        }
    ")]
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    [GraphQLMetadata("Query")]
    [GraphTypeMetadata(@"
        extend type Query {
            viewer: User
        }
    ")]
    public class QueryType
    {
        [GraphQLAuthorize("CustomRequirement")]
        public User Viewer()
        {
            return new User { Id = 1, Name = "Quinn" };
        }
    }
}