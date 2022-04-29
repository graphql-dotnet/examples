using GraphQL;
using GraphQL.MicrosoftDI;
using GraphQL.Server;
using GraphQL.SystemTextJson;
using GraphQL.Types;
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
                .AddHttpMiddleware<ISchema>()
                .AddUserContextBuilder(httpContext => new GraphQLUserContext { User = httpContext.User })
                .AddSystemTextJson()
                .AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = true)
                .AddSchema<StarWarsSchema>()
                .AddGraphTypes(typeof(StarWarsSchema).Assembly));

            services.AddSingleton<StarWarsData>();
            services.AddLogging(builder => builder.AddConsole());
            services.AddHttpContextAccessor();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseGraphQL<ISchema>();
            app.UseGraphQLPlayground();
        }
    }
}
