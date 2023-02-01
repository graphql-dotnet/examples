using Example.Repositories;
using GraphQL;
using GraphQL.Resolvers;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;

namespace Example.GraphQL
{
    public interface IQueryFieldsProvider
    {
        void AddQueryFields(ObjectGraphType objectGraph);
    }

    public interface IMutationFieldsProvider
    {
        void AddMutationFields(ObjectGraphType objectGraph);
    }

    public interface IDogQuery : IQueryFieldsProvider { }

    public interface ICatQuery : IQueryFieldsProvider { }

    public interface ICatMutation : IMutationFieldsProvider { }

    public class DogQuery : IDogQuery
    {
        private readonly IServiceProvider _serviceProvider;

        public DogQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void AddQueryFields(ObjectGraphType objectGraph)
        {
            objectGraph.Field<StringGraphType>("say", resolve: context => "woof woof woof");

            objectGraph.Field<NonNullGraphType<ListGraphType<NonNullGraphType<DogType>>>>("dogBreeds", resolve: context =>
            {
                using var scope = _serviceProvider.CreateScope();
                var dogRepository = scope.ServiceProvider.GetRequiredService<DogRepository>();
                var dogs = dogRepository.GetDogs();
                return dogs;
            });
        }
    }

    public class DogImageDetailsQuery : IDogQuery
    {
        private readonly IServiceProvider _serviceProvider;

        public DogImageDetailsQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void AddQueryFields(ObjectGraphType objectGraph)
        {
            objectGraph.FieldAsync<NonNullGraphType<ImageDetailsType>>("dogImageDetails", resolve: async context =>
            {
                using var scope = _serviceProvider.CreateScope();
                var imageDetailsRepository = scope.ServiceProvider.GetRequiredService<DogImageDetailsRepository>();
                var imageDetails = await imageDetailsRepository.GetDogImageDetails();
                return imageDetails;
            });
        }
    }

    public class CatQuery : ICatQuery
    {
        public void AddQueryFields(ObjectGraphType objectGraph)
        {
            objectGraph.Field<StringGraphType>("say", resolve: context => "meow meow meow");
        }
    }

    public class CatBreedUpdateMutation : ICatMutation
    {
        private readonly IServiceProvider _serviceProvider;

        public CatBreedUpdateMutation(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void AddMutationFields(ObjectGraphType objectGraph)
        {
            var args = new QueryArguments
            {
                new QueryArgument<StringGraphType> { Name = "breedName" },
                new QueryArgument<StringGraphType> { Name = "newBreedName" }
            };

            objectGraph.Field<CatType>("updateCatBreed", arguments: args, resolve: context =>
            {
                var breed = context.GetArgument<string>("breedName");
                var newBreed = context.GetArgument<string>("newBreedName");
                using var scope = _serviceProvider.CreateScope();
                var catRepository = scope.ServiceProvider.GetRequiredService<CatRepository>();
                var result = catRepository.UpdateCatBreedName(breed, newBreed);

                return result;
            });
        }
    }
}
