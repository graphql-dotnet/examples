using Example.Repositories;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Example.GraphQL
{
    public interface IQueryFieldsProvider
    {
        void AddQueryFields(ObjectGraphType objectGraph);
    }

    public interface IDogQuery : IQueryFieldsProvider { }

    public interface ICatQuery : IQueryFieldsProvider { }

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
        private readonly IServiceProvider _serviceProvider;

        public CatQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void AddQueryFields(ObjectGraphType objectGraph)
        {
            objectGraph.Field<StringGraphType>("say", resolve: context => "meow meow meow");

            objectGraph.Field<NonNullGraphType<ListGraphType<NonNullGraphType<CatType>>>>("catBreeds", resolve: context =>
            {
                using var scope = _serviceProvider.CreateScope();
                var catRepository = scope.ServiceProvider.GetRequiredService<CatRepository>();
                var cats = catRepository.GetCats();
                return cats;
            });
        }
    }
}
