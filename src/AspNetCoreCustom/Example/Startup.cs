using GraphQL;
using GraphQL.MicrosoftDI;
using GraphQL.SystemTextJson;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StarWars;

namespace Example
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGraphQL(b => b
                .AddSchema<StarWarsSchema>()
                .AddSystemTextJson()
                .AddValidationRule<InputValidationRule>()
                .AddGraphTypes(typeof(StarWarsSchema).Assembly)
                .AddMetrics(_ => false, (services, _) => services.GetRequiredService<GraphQLSettings>().EnableMetrics));

            services.AddSingleton<StarWarsData>();
            services.AddSingleton<GraphQLMiddleware>();
            services.AddSingleton(new GraphQLSettings
            {
                Path = "/api/graphql",
                BuildUserContext = ctx => new GraphQLUserContext
                {
                    User = ctx.User
                },
                EnableMetrics = true
            });
            services.AddLogging(builder => builder.AddConsole());
            services.AddHttpContextAccessor();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseMiddleware<GraphQLMiddleware>();

            app.UseDefaultFiles();
            app.UseStaticFiles();
        }
    }
}
