using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace Example
{
    public class GraphQLUserContext : Dictionary<string, object>
    {
        public ClaimsPrincipal User { get; set; }
    }

    public class GraphQLRequest
    {
        [JsonPropertyName("operationName")]
        public string OperationName { get; set; }

        [JsonPropertyName("query")]
        public string Query { get; set; }

        [JsonPropertyName("variables")]
        public GraphQL.Inputs Variables { get; set; }
    }

    public class GraphQLSettings
    {
        public PathString Path { get; set; } = "/api/graphql";

        public Func<HttpContext, IDictionary<string, object>> BuildUserContext { get; set; }

        public bool EnableMetrics { get; set; }
    }
}