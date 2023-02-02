namespace Example;

public interface IBreed
{
    public string Breed { get; set; }
}

public class Dog : IBreed
{
    public string Breed { get; set; }
}

public class Cat : IBreed
{
    public string Breed { get; set; }
}

public class ImageDetails
{
    public string Url { get; set; }
}
