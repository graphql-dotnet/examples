using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Example
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<DogSchema>();
            services.AddSingleton<DogQuery>();
            services.AddSingleton<CatSchema>();
            services.AddSingleton<CatQuery>();

            services.AddLogging(builder => builder.AddConsole());
            services.AddHttpContextAccessor();

#pragma warning disable CS0612 // Type or member is obsolete
            services
                .AddGraphQL(options => options.EnableMetrics = true)
                .AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = true)
                .AddSystemTextJson()
                .AddUserContextBuilder(httpContext => new GraphQLUserContext { User = httpContext.User });
#pragma warning restore CS0612 // Type or member is obsolete
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseGraphQL<DogSchema>("/api/dogs");
            app.UseGraphQL<CatSchema>("/api/cats");

            app.UseGraphQLPlayground(new PlaygroundOptions { GraphQLEndPoint = "/api/dogs" }, "/ui/dogs");
            app.UseGraphQLPlayground(new PlaygroundOptions { GraphQLEndPoint = "/api/cats" }, "/ui/cats");
        }
    }
}
