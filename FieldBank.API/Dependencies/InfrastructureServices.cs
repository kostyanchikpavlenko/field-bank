using FluentValidation;
using System.Reflection;

namespace FieldBank.API.Dependencies
{
    public static class InfrastructureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            Assembly assembly = typeof(Program).Assembly;
            
            services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly));
            services.AddValidatorsFromAssembly(assembly);
            
            return services;
        }
    }
}
