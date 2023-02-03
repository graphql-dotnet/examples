using System.Threading.Tasks;
using GraphQL.Validation;
using GraphQLParser.AST;

namespace StarWars;

public class InputValidationRule : IValidationRule
{
    public ValueTask<INodeVisitor> ValidateAsync(ValidationContext context)
    {
        return new ValueTask<INodeVisitor>(new MatchingNodeVisitor<GraphQLField>(
            (field, context2) => { },
            (field, context2) => { }));
    }
}
