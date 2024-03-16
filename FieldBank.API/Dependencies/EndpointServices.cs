using FieldBank.API.Common.Endpoints;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace FieldBank.API.Dependencies
{
    public static class EndpointServices
    {
        public static IServiceCollection AddEndpoints(this IServiceCollection services)
        {
            var assebmly = typeof(Program).Assembly;
            
            var serviceDescriptors = assebmly
                .DefinedTypes
                .Where(type => !type.IsAbstract
                               && !type.IsInterface
                               && type.IsAssignableTo(typeof(IEndpoint)))
                .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type));
            
            services.TryAddEnumerable(serviceDescriptors);
            
            return services;
        }

        public static IApplicationBuilder MapMinimalEndpoints(this WebApplication app)
        {
            var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

            foreach (var endpoint in endpoints)
            {
                endpoint.MapEndpoint(app);
            }

            return app;
        }
    }
}