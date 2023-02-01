using GraphQL.Types;
using System.Collections.Generic;

namespace Example.GraphQL
{
    public class Queries
    {
        public class DogQuery : ObjectGraphType<object>
        {
            public DogQuery(IEnumerable<IDogOperation> dogOperations)
            {
                foreach (var dogOperation in dogOperations)
                {
                    var fields = dogOperation.GetFields();
                    foreach (var field in fields)
                    {
                        AddField((FieldType)field);
                    }
                }
            }
        }

        public class CatQuery : ObjectGraphType<object>
        {
            public CatQuery(IEnumerable<ICatOperation> catOperations)
            {
                foreach (var catOperation in catOperations)
                {
                    var fields = catOperation.GetFields();
                    foreach (var field in fields)
                    {
                        AddField((FieldType)field);
                    }
                }
            }
        }
    }
}
