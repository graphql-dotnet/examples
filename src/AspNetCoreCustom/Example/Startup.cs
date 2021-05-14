using GraphQL;
using GraphQL.Server.Ui.Playground;
using GraphQL.Types;
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
            services.AddLogging(builder => builder.AddConsole());
            services.AddHttpContextAccessor();

            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddSingleton<IDocumentWriter, GraphQL.SystemTextJson.DocumentWriter>();

            services.AddSingleton<ISchema>(s =>
            {
                return Schema.For(@"
                    type Query {
                        hello: String
                    }
                ",
                _ => _.Types.Include<QueryType>());
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseGraphQLPlayground(new PlaygroundOptions { GraphQLEndPoint = "/api/graphql" });

            app.UseMiddleware<GraphQLMiddleware>(new GraphQLSettings
            {
                Path = "/api/graphql",
                BuildUserContext = ctx => new GraphQLUserContext
                {
                    User = ctx.User
                },
                EnableMetrics = true
            });
        }
    }
}
