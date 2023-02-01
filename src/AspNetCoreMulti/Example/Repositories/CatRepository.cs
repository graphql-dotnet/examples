namespace Example.Repositories;

using System.Collections.Generic;
using System.Linq;

public class CatRepository
{
    private static readonly List<Cat> Cats = new()
        {
            new Cat { Breed = "Abyssinian" },
            new Cat { Breed = "American Bobtail" },
            new Cat { Breed = "Burmese" }
        };

    public IEnumerable<Cat> GetCats() => Cats.AsEnumerable();

    public Cat UpdateCatBreedName(string oldName, string newName)
    {
        var match = Cats.FirstOrDefault(x => x.Breed == oldName);
        if (match == null)
        {
            throw new System.Exception("Cannot find that cat !");
        }

        match.Breed = newName;

        return match;
    }
}
