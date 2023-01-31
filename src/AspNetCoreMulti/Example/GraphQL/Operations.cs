using Example.Repositories;
using GraphQL.Resolvers;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Example.GraphQL
{
    public interface IOperation
    {
        IEnumerable<IFieldType> RegisterFields();
    }

    public interface IDogOperation : IOperation { }

    public interface ICatOperation : IOperation { }

    public class DogOperation : ObjectGraphType<object>, IDogOperation
    {
        private readonly IServiceProvider _serviceProvider;

        public DogOperation(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IEnumerable<IFieldType> RegisterFields()
        {
            var fields = new List<IFieldType>
            {
                Field<StringGraphType>("say", resolve: context => "woof woof woof"),
                GetBreedListField(),
                GetImageDetailsField()
            };

            return fields;
        }

        private IFieldType GetBreedListField()
        {
            return Field<NonNullGraphType<ListGraphType<NonNullGraphType<DogType>>>>("dogBreeds", resolve: context =>
            {
                using var scope = _serviceProvider.CreateScope();
                var dogRepository = scope.ServiceProvider.GetRequiredService<DogRepository>();
                var dogs = dogRepository.GetDogs();
                return dogs;
            });
        }

        private IFieldType GetImageDetailsField()
        {
            return FieldAsync<NonNullGraphType<ImageDetailsType>>("dogImageDetails", resolve: async context =>
            {
                using var scope = _serviceProvider.CreateScope();
                var imageDetailsRepository = scope.ServiceProvider.GetRequiredService<DogImageDetailsRepository>();
                var imageDetails = await imageDetailsRepository.GetDogImageDetails();
                return imageDetails;
            });
        }
    }

    public class CatSayOperation : ObjectGraphType<object>, ICatOperation
    {
        public IEnumerable<IFieldType> RegisterFields()
        {
            return new List<IFieldType> { Field<StringGraphType>("say", resolve: context => "meow meow meow") };
        }
    }
}
