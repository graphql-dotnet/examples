using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Example.Repositories;
public class DogImageDetailsRepository
{
    private readonly IHttpClientFactory _httpClientFactory;

    public DogImageDetailsRepository(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<ImageDetails> GetDogImageDetailsAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("DogsApi");
            var result = await client.GetStringAsync("api/breeds/image/random");
            var apiResponse = JsonSerializer.Deserialize<DogsImageApiResponse>(result);

            return new ImageDetails { Url = apiResponse.Message };
        }
        catch (Exception ex)
        {
            return new ImageDetails { Url = ex.Message };
        }
    }

    private class DogsImageApiResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
