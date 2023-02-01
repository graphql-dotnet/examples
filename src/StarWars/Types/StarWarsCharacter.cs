namespace StarWars.Types;

public abstract class StarWarsCharacter
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string[] Friends { get; set; }
    public int[] AppearsIn { get; set; }
}

public class Human : StarWarsCharacter
{
    public string HomePlanet { get; set; }
}

public class Droid : StarWarsCharacter
{
    public string PrimaryFunction { get; set; }
}
