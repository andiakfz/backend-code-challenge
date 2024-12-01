using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using portlocator.Application.Abstraction.Validation;
using portlocator.Application.Ships.Update.UpdateVelocity;
using portlocator.Infrastructure.Database;
using portlocator.Infrastructure.Seeder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portlocator.Tests
{
    public class Setup : IAsyncLifetime
    {
        public IServiceProvider ServiceProvider { get; }

        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConfiguration _configuration;

        public Setup()
        {
            _configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                       .AddJsonFile("testsettings.json", optional: true, reloadOnChange: true)
                                                       .Build();

            var serviceCollection = new ServiceCollection();
            string connString = _configuration.GetConnectionString("Database")!;

            serviceCollection.AddLogging();

            serviceCollection.AddDbContext<AppDbContext>((provider, options) =>
            {
                options.UseNpgsql(connString).UseSnakeCaseNamingConvention();
            });

            // Setup Mediatr
            serviceCollection.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(portlocator.Application.RegisterInjection).Assembly);

                config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            });
            serviceCollection.AddValidatorsFromAssembly(typeof(portlocator.Application.RegisterInjection).Assembly, includeInternalTypes: true);

            ServiceProvider = serviceCollection.BuildServiceProvider();
            _serviceScopeFactory = ServiceProvider.GetRequiredService<IServiceScopeFactory>();
        }


        public async Task InitializeAsync()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                dbContext.Database.ExecuteSqlRaw("CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\"");

                //await dbContext.Database.EnsureCreatedAsync();
                await dbContext.Database.MigrateAsync();

                // Seed Default
                var seeder = new TestSeeder(dbContext);
                await seeder.SeedAsync();
            }
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

    }
}
