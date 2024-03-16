namespace FieldBank.API.Dependencies
{
    public static class InfrastructureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Program).Assembly));
            return services;
        }
    }
}
