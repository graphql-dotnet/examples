using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace Example
{
    public class Program
    {
        public static Task Main(string[] args)
        {
            try
            {
                return Host
                    .CreateDefaultBuilder(args)
                    .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>())
                    .Build()
                    .RunAsync();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                return Task.FromResult(0);
            }
        }
    }
}
