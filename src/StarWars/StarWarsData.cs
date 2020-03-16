using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraphQL;
using StarWars.Types;

namespace StarWars
{
    public class StarWarsData
    {
        private readonly List<Human> _humans = new List<Human>();
        private readonly List<Droid> _droids = new List<Droid>();
        private readonly List<Planet> _planets = new List<Planet>();

        public StarWarsData()
        {
            _humans.Add(new Human
            {
                Id = "1",
                Name = "Luke",
                Friends = new[] { "3", "4" },
                AppearsIn = new[] { 4, 5, 6 },
                HomePlanet = "Tatooine",
                FavoriteDroid = "3"
            });
            _humans.Add(new Human
            {
                Id = "2",
                Name = "Vader",
                AppearsIn = new[] { 4, 5, 6 },
                HomePlanet = "Tatooine",
                FavoriteDroid = "4"
            });

            _droids.Add(new Droid
            {
                Id = "3",
                Name = "R2-D2",
                Friends = new[] { "1", "4" },
                AppearsIn = new[] { 4, 5, 6 },
                PrimaryFunction = "Astromech",
                ManufacturdOn = "Naboo"
            });
            _droids.Add(new Droid
            {
                Id = "4",
                Name = "C-3PO",
                AppearsIn = new[] { 4, 5, 6 },
                PrimaryFunction = "Protocol",
                ManufacturdOn = "Tatooine"
            });
            _planets.Add(new Planet
            {
                Id = "Tatooine",
                Name = "Tatooine",
                MostFamousJedi = "1",
                MostFamousSith = "2"
            });
            _planets.Add(new Planet
            {
                Id = "Naboo",
                Name = "Naboo",
                MostFamousJedi = null,
                MostFamousSith = null
            });
        }

        public IEnumerable<StarWarsCharacter> GetFriends(StarWarsCharacter character)
        {
            if (character == null)
            {
                return null;
            }

            var friends = new List<StarWarsCharacter>();
            var lookup = character.Friends;
            if (lookup != null)
            {
                _humans.Where(h => lookup.Contains(h.Id)).Apply(friends.Add);
                _droids.Where(d => lookup.Contains(d.Id)).Apply(friends.Add);
            }
            return friends;
        }

        public Task<Human> GetHumanByIdAsync(string id)
        {
            return Task.FromResult(_humans.FirstOrDefault(h => h.Id == id));
        }

        public Task<Droid> GetDroidByIdAsync(string id)
        {
            return Task.FromResult(_droids.FirstOrDefault(h => h.Id == id));
        }

        private static int _GetPlanetByNameAsync = 0;
        public async Task<Planet> GetPlanetByNameAsync(string id)
        {
            await Task.Delay(1000);
            Console.WriteLine(Interlocked.Increment(ref _GetPlanetByNameAsync));
            return _planets.FirstOrDefault(h => h.Name == id);
        }

        public Human AddHuman(Human human)
        {
            human.Id = Guid.NewGuid().ToString();
            _humans.Add(human);
            return human;
        }
    }
}
