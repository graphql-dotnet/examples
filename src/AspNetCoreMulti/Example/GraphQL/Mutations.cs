using Example.Repositories;
using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Example.GraphQL;

public interface IMutationFieldsProvider
{
    void AddMutationFields(ObjectGraphType objectGraph);
}

public interface ICatMutation : IMutationFieldsProvider { }

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
