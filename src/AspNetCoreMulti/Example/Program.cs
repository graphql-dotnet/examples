using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Example
{
    public class Program
    {
        public static Task Main(string[] args) => Host
            .CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>())
            .Build()
            .RunAsync();
    }
}
