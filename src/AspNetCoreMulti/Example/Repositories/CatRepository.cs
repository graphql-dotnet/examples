using System.Collections.Generic;
using System.Linq;

namespace Example.Repositories
{
    public class CatRepository
    {
        private static readonly List<Cat> Cats = new()
        {
            new Cat{ Breed = "Abyssinian" },
            new Cat{ Breed = "American Bobtail" },
            new Cat{ Breed = "Burmese" }
        };

        public IEnumerable<Cat> GetCats() => Cats.AsEnumerable();
    }
}
