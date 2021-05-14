using System;
using System.Linq;
using GraphQL;
using GraphQL.Authorization;
using GraphQL.Server.Ui.Playground;
using GraphQL.Types;
using GraphQL.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            services.AddAuthorization();
            services.AddHttpContextAccessor();

            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddSingleton<IDocumentWriter, GraphQL.SystemTextJson.DocumentWriter>();

            services.AddSingleton<IAuthorizationEvaluator, AuthorizationEvaluator>();
            services.AddTransient<IValidationRule, AuthorizationValidationRule>();
            services.AddTransient<UserIsJoeRequirement>();

            services.AddTransient(s =>
            {
                var authSettings = new AuthorizationSettings();
                // add a policy that checks claims on the Authenticated User
                authSettings.AddPolicy("AdminPolicy", p => p.RequireClaim("role", "Admin"));

                // add a policy which uses a custom AuthorizationRequirement
                authSettings.AddPolicy("CustomRequirement", p => p.AddRequirement(s.GetRequiredService<UserIsJoeRequirement>()));
                return authSettings;
            });

            services.AddSingleton<ISchema>(s =>
            {
                // get all types in assembly that have the GraphTypeMetadataAttribute attribute
                // use the schema-first type definitions to build the schema
                var types = typeof(QueryType).Assembly.WithAttribute<GraphTypeMetadataAttribute>();
                var typeDefs = types
                    .SelectMany(x => x.GetCustomAttributes<GraphTypeMetadataAttribute>())
                    .Select(x => x.TypeDef)
                    .ToArray();

                var schemaFirst = string.Join(Environment.NewLine, typeDefs);

                return Schema.For(schemaFirst, _ =>
                {
                    foreach (var type in types)
                    {
                        _.Types.Include(type);
                    }
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthorization();

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
