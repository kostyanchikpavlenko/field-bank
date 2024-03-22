using FieldBank.API.Persistence.Interfaces;
using FieldBank.API.Persistence.Services;

namespace FieldBank.API.Dependencies
{
    public static class PersistenceServices
    {
        public static IServiceCollection AddDatabaseServices(this IServiceCollection services)
        {
            services.AddSingleton<SqlConnectionFactory>(config =>
            {
                var configuration = config.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("DefaultConnectionString") ??
                                       throw new ApplicationException("No connection string found");
                
                return new SqlConnectionFactory(connectionString);
            });

            services.AddTransient<ISqlProvider, SqlProvider>();

            return services;
        }
    }
}