using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace Example
{
    public class Program
    {
        public static Task Main(string[] args) => WebHost
            .CreateDefaultBuilder<Startup>(args)
            .UseWebRoot(Path.Combine(Directory.GetCurrentDirectory(), "public"))
            .Build()
            .RunAsync();
    }
}
