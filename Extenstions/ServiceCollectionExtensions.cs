using AoiCryptoAPI.Data;
using AoiCryptoAPI.Models;
using AoiCryptoAPI.Services;
using MongoDB.Driver;

namespace AoiCryptoAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMongoDB(this IServiceCollection services, string connectionString, string databaseName)
        {
            // Configure MongoDB Client
            services.AddSingleton<IMongoClient>(sp =>
                new MongoClient(connectionString));

            // Configure MongoDB Database
            services.AddSingleton(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase(databaseName);
            });
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            // Register Repositories
            services.AddSingleton(sp =>
                new MongoRepository<Project>(sp.GetRequiredService<IMongoDatabase>(), "Projects"));

            services.AddSingleton(sp =>
                new MongoRepository<Token>(sp.GetRequiredService<IMongoDatabase>(), "Token"));

            services.AddSingleton(sp =>
                new MongoRepository<AllowlistEntry>(sp.GetRequiredService<IMongoDatabase>(), "AllowlistEntry"));
        }

        public static void AddServices(this IServiceCollection services)
        {
            // Register Services
            services.AddScoped<ProjectService>();
            services.AddScoped<AllowlistService>();
            services.AddScoped<TokenService>();
        }
    }
}
