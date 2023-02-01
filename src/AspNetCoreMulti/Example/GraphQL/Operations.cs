using Example.Repositories;
using GraphQL.Resolvers;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;

namespace Example.GraphQL
{
    public interface IOperation
    {
        IEnumerable<IFieldType> GetFields();
    }

    public interface IDogOperation : IOperation { }

    public interface ICatOperation : IOperation { }

    public class DogOperation : IDogOperation
    {
        private readonly IServiceProvider _serviceProvider;

        public DogOperation(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IEnumerable<IFieldType> GetFields()
        {
            var fields = new List<IFieldType>();

            var sayField = new FieldType
            {
                Name = "say",
                Type = typeof(StringGraphType),
                Resolver = new FuncFieldResolver<object, object>(context => "woof woof woof")
            };

            fields.Add(sayField);

            var breedListField = new FieldType
            {
                Name = "dogBreeds",
                Type = typeof(NonNullGraphType<ListGraphType<NonNullGraphType<DogType>>>),
                Resolver = new FuncFieldResolver<object, object>(context =>
                {
                    using var scope = _serviceProvider.CreateScope();
                    var dogRepository = scope.ServiceProvider.GetRequiredService<DogRepository>();
                    var dogs = dogRepository.GetDogs();
                    return dogs;
                })
            };

            fields.Add(breedListField);

            var imageDetailsField = new FieldType
            {
                Name = "dogImageDetails",
                Type = typeof(NonNullGraphType<ImageDetailsType>),
                Resolver = new FuncFieldResolver<object, object>(async context =>
                {
                    using var scope = _serviceProvider.CreateScope();
                    var imageDetailsRepository = scope.ServiceProvider.GetRequiredService<DogImageDetailsRepository>();
                    var imageDetails = await imageDetailsRepository.GetDogImageDetails();
                    return imageDetails;
                })
            };

            fields.Add(imageDetailsField);

            return fields;
        }
    }

    public class CatSayOperation : ICatOperation
    {
        public IEnumerable<IFieldType> GetFields()
        {
            var fields = new List<IFieldType>();
            var sayField = new FieldType
            {
                Name = "say",
                Type = typeof(StringGraphType),
                Resolver = new FuncFieldResolver<object, object>(context => "meow meow meow")
            };

            fields.Add(sayField);

            return fields;
        }
    }
}
