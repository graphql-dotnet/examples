using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                using (var cts = new CancellationTokenSource())
                {
                    ConsoleCancelEventHandler handler = (o, e) =>
                    {
                        Console.WriteLine("Cancelling...");
                        cts.Cancel();
                        e.Cancel = true;
                    };
                    Console.CancelKeyPress += handler;
                    try
                    {
                        Console.WriteLine("Searching for city 'detroit' (press Ctrl-C to cancel)...");
                        var result = await CallGraphQLAsync<MyResponseData>(
                            new Uri("https://graphql-weather-api.herokuapp.com/"),
                            HttpMethod.Post,
                            "query ($city: String!) { getCityByName(name: $city) { name country } }",
                            new
                            {
                                city = "detroit",
                            },
                            cts.Token);
                        Console.WriteLine($"Found city {result.Data.GetCityByName.Name}, {result.Data.GetCityByName.Country}");
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
                Console.WriteLine($"Caught exception: {ex}");
            }
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }

        private class MyResponseData
        {
            public MyResponseGetCityByName GetCityByName { get; set; }
        }

        private class MyResponseGetCityByName
        {
            public string Name { get; set; }
            public string Country { get; set; }
        }

        private static HttpClient httpClient = new HttpClient();

        static async Task<GraphQLResponse<TResponse>> CallGraphQLAsync<TResponse>(Uri endpoint, HttpMethod method, string query, object variables, CancellationToken cancellationToken)
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
            using (var response = await httpClient.SendAsync(httpRequestMessage, cancellationToken))
            {
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync(); //cancellationToken supported for .NET 5/6
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
