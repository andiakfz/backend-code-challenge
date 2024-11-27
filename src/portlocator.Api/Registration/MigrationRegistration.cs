using Microsoft.EntityFrameworkCore;
using portlocator.Infrastructure.Database;
using portlocator.Infrastructure.Seeder;

namespace portlocator.Api.Registration
{
    public static class MigrationRegistration
    {
        public static void ApplyMigrations(this IApplicationBuilder app, IConfiguration configuration)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // CHECK "uuid-ossp" EXTENSION FOR GUID CREATION
            dbContext.Database.ExecuteSqlRaw("CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\"");

            dbContext.Database.Migrate();

            // SEED DATABASE
            var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
            seeder.SeedAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
