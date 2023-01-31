using System.Net.Http;
using System.Threading.Tasks;

namespace Example.Repositories
{
    public class DogImageDetailsRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DogImageDetailsRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ImageDetails> GetDogImageDetails()
        {
            var client = _httpClientFactory.CreateClient("DogsApi");
            var result = await client.GetStringAsync("api/breeds/image/random");

            return new ImageDetails { Url = result };
        }
    }
}
