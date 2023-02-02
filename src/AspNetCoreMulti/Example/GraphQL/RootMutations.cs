using GraphQL.Types;
using System.Collections.Generic;

namespace Example.GraphQL;

public class CatRootMutation : ObjectGraphType
{
    public CatRootMutation(IEnumerable<ICatMutation> mutations)
    {
        foreach (var mutation in mutations)
        {
            mutation.AddMutationFields(this);
        }
    }
}
