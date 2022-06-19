using GraphQL;
using GraphQL.Caching;
using GraphQL.MicrosoftDI;
using GraphQL.SystemTextJson;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StarWars;

namespace Example
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGraphQL(b => b
                .AddSchema<StarWarsSchema>()
                .AddSystemTextJson()
                .AddValidationRule<InputValidationRule>()
                .AddGraphTypes(typeof(StarWarsSchema).Assembly)
                .AddMemoryCache()
                .AddApolloTracing(options => options.RequestServices!.GetRequiredService<IOptions<GraphQLSettings>>().Value.EnableMetrics));

            services.Configure<GraphQLSettings>(Configuration.GetSection("GraphQLSettings"));
            services.AddSingleton<StarWarsData>();
            services.AddLogging(builder => builder.AddConsole());
            services.AddHttpContextAccessor();
            services.AddControllersWithViews()
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.Converters.Add(new InputsJsonConverter());
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();
        }
    }
}
