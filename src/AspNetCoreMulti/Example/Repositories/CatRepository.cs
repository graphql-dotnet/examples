using System.Collections.Generic;
using System.Linq;

namespace Example.Repositories;
public class CatRepository
{
    private static readonly List<Cat> _cats = new()
    {
        new Cat { Breed = "Abyssinian" },
        new Cat { Breed = "American Bobtail" },
        new Cat { Breed = "Burmese" }
    };

    public IEnumerable<Cat> GetCats() => _cats.AsEnumerable();

    public Cat UpdateCatBreedName(string oldName, string newName)
    {
        var match = _cats.FirstOrDefault(x => x.Breed == oldName);
        if (match == null)
        {
            throw new System.Exception("Cannot find that cat !");
        }

        match.Breed = newName;

        return match;
    }
}
