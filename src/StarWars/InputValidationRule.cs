using GraphQL.Validation;
using GraphQLParser.AST;
using System.Threading.Tasks;

namespace StarWars
{
    public class InputValidationRule : IValidationRule
    {
        public ValueTask<INodeVisitor> ValidateAsync(ValidationContext context)
        {
            return new ValueTask<INodeVisitor>(new MatchingNodeVisitor<GraphQLField>(
                (field, context2) => { },
                (field, context2) => { }));
        }
    }
}
