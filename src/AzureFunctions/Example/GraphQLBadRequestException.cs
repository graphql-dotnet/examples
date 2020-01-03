// <copyright file="GraphQLExecuterExtensions.cs">
// MIT License, taken from https://github.com/tpeczek/Demo.Azure.Functions.GraphQL/blob/master/Demo.Azure.Functions.GraphQL/Infrastructure/GraphQLBadRequestException.cs
// </copyright>
// <author>https://github.com/tpeczek</author>
using System;

namespace Example
{
    internal class GraphQLBadRequestException : Exception
    {
        public GraphQLBadRequestException(string message)
            : base(message)
        { }
    }
}
