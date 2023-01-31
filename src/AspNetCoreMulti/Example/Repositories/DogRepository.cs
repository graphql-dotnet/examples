using System.Collections.Generic;
using System.Linq;

namespace Example.Repositories
{
    public class DogRepository
    {
        private static readonly List<Dog> Dogs = new()
        {
            new Dog{ Breed = "Doberman" },
            new Dog{ Breed = "Pit Bull" },
            new Dog{ Breed = "German Shepard" }
        };

        public IEnumerable<Dog> GetDogs() => Dogs.AsEnumerable();
    }
}
