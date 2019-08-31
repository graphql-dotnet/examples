using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace Example
{
    public class Program
    {
        public static Task Main(string[] args) => WebHost.CreateDefaultBuilder<Startup>(args).Build().RunAsync();
    }
}
