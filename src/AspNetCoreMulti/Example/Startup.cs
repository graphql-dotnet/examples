using GraphQL;
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
            GraphQL.MicrosoftDI.GraphQLBuilderExtensions.AddGraphQL(services)
                .AddServer(true, opt => opt.EnableMetrics = true)
                .AddUserContextBuilder(httpContext => new GraphQLUserContext { User = httpContext.User })
                .AddSchema<CatSchema>()
                .AddSchema<DogSchema>()
                .AddSystemTextJson()
                .AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = true)
                .AddGraphTypes();

            services.AddLogging(builder => builder.AddConsole());
            services.AddHttpContextAccessor();
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
