using GraphQL.Types;

namespace Example.GraphQL
{
    public class DogType : ObjectGraphType<Dog>
    {
        public DogType()
        {
            Field(x => x.Breed);
        }
    }

    public class CatType : ObjectGraphType<Cat>
    {
        public CatType()
        {
            Field(x => x.Breed);
        }
    }

    public class ImageDetailsType : ObjectGraphType<ImageDetails>
    {
        public ImageDetailsType()
        {
            Field(x => x.Url);
        }
    }
}
