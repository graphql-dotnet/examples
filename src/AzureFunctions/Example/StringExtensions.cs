using GraphQL;
using GraphQL.NewtonsoftJson;
using Newtonsoft.Json.Linq;

public static class StringExtensions
{
    private static readonly GraphQLSerializer _serializer = new();

    public static Inputs ToInputs(this string json)
        => json == null ? Inputs.Empty : _serializer.Deserialize<Inputs>(json) ?? Inputs.Empty;

    public static Inputs ToInputs(this JObject element)
        => _serializer.ReadNode<Inputs>(element) ?? Inputs.Empty;

    public static T? FromJson<T>(this string json)
        => _serializer.Deserialize<T>(json);

    public static System.Threading.Tasks.ValueTask<T?> FromJsonAsync<T>(this System.IO.Stream stream, System.Threading.CancellationToken cancellationToken = default)
        => _serializer.ReadAsync<T>(stream, cancellationToken);
}
