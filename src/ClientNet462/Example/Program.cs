using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Client
{
    internal class Program
    {
        /// <summary>
        /// Program entry point with wire-up for Ctrl-C handler and exception handling.
        /// </summary>
        private static async Task Main(string[] args)
        {
            try
            {
                using (var cts = new CancellationTokenSource())
                {
                    ConsoleCancelEventHandler handler = (o, e) =>
                    {
                        Console.WriteLine("Cancelling...");
                        cts.CancelAfter(0);
                        e.Cancel = true;
                    };
                    Console.CancelKeyPress += handler;
                    try
                    {
                        await Main2(cts.Token);
                    }
                    finally
                    {
                        Console.CancelKeyPress -= handler;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Cancelled query");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Caught exception: {ex.Message}");
            }
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }

        /// <summary>
        /// Sample method which calls a GraphQL endpoint.
        /// </summary>
        private static async Task Main2(CancellationToken cancellationToken)
        {
            Console.WriteLine("Searching for city 'detroit' (press Ctrl-C to cancel)...");

            // Call GraphQL endpoint here, specifying return data type, endpoint, method, query, and variables
            var result = await CallGraphQLAsync<MyResponseData>(
                new Uri("https://graphql-weather-api.herokuapp.com/"),
                HttpMethod.Post,
                "query ($city: String!) { getCityByName(name: $city) { name country } }",
                new
                {
                    city = "detroit",
                },
                cancellationToken);

            // Examine the GraphQL response to see if any errors were encountered
            if (result.Errors?.Count > 0)
            {
                Console.WriteLine($"GraphQL returned errors:\n{string.Join("\n", result.Errors.Select(x => $"  - {x.Message}"))}");
                return;
            }

            // Use the response data
            Console.WriteLine($"Found city {result.Data.GetCityByName.Name}, {result.Data.GetCityByName.Country}");
        }

        /// <summary>
        /// Sample data class for expected response.
        /// </summary>
        private class MyResponseData
        {
            public MyResponseGetCityByName GetCityByName { get; set; }
        }

        private class MyResponseGetCityByName
        {
            public string Name { get; set; }
            public string Country { get; set; }
        }

        private static readonly HttpClient _httpClient = new HttpClient();

        /// <summary>
        /// Calls a specified GraphQL endpoint with the specified query and variables.
        /// </summary>
        private static async Task<GraphQLResponse<TResponse>> CallGraphQLAsync<TResponse>(Uri endpoint, HttpMethod method, string query, object variables, CancellationToken cancellationToken)
        {
            var content = new StringContent(SerializeGraphQLCall(query, variables), Encoding.UTF8, "application/json");
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = method,
                Content = content,
                RequestUri = endpoint,
            };
            //add authorization headers if necessary here
            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            using (var response = await _httpClient.SendAsync(httpRequestMessage, cancellationToken).ConfigureAwait(false))
            {
                //if (response.IsSuccessStatusCode)
                if (response?.Content.Headers.ContentType?.MediaType == "application/json")
                {
                    var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false); //cancellationToken supported for .NET 5/6
                    return DeserializeGraphQLCall<TResponse>(responseString);
                }
                else
                {
                    throw new ApplicationException($"Unable to contact '{endpoint}': {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
        }

        public class GraphQLErrorLocation
        {
            public int Line { get; set; }
            public int Column { get; set; }
        }

        public class GraphQLError
        {
            public string Message { get; set; }
            public List<GraphQLErrorLocation> Locations { get; set; }
            public List<object> Path { get; set; } //either int or string
        }

        public class GraphQLResponse<TResponse>
        {
            public List<GraphQLError> Errors { get; set; }
            public TResponse Data { get; set; }
        }

        /// <summary>
        /// Serializes a query and variables to JSON to be sent to the GraphQL endpoint.
        /// </summary>
        private static string SerializeGraphQLCall(string query, object variables)
        {
            var sb = new StringBuilder();
            var textWriter = new StringWriter(sb);
            var serializer = new JsonSerializer();
            serializer.Serialize(textWriter, new
            {
                query = query,
                variables = variables,
            });
            return sb.ToString();
        }

        /// <summary>
        /// Deserializes a GraphQL response.
        /// </summary>
        private static GraphQLResponse<TResponse> DeserializeGraphQLCall<TResponse>(string response)
        {
            var serializer = new JsonSerializer();
            var stringReader = new StringReader(response);
            var jsonReader = new JsonTextReader(stringReader);
            var result = serializer.Deserialize<GraphQLResponse<TResponse>>(jsonReader);
            return result;
        }
    }
}
