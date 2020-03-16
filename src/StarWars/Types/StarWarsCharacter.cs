namespace StarWars.Types
{
    public abstract class StarWarsNode
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public abstract class StarWarsCharacter : StarWarsNode
    {
        public string[] Friends { get; set; }
        public int[] AppearsIn { get; set; }
    }

    public class Human : StarWarsCharacter
    {
        public string HomePlanet { get; set; }
        public string FavoriteDroid { get; internal set; }
    }

    public class Droid : StarWarsCharacter
    {
        public string PrimaryFunction { get; set; }
        public string ManufacturdOn { get; set; }
    }

    public class Planet : StarWarsNode
    {
        public string MostFamousSith { get; set; }
        public string MostFamousJedi { get; set; }
    }
}
