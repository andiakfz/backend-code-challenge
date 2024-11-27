using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using portlocator.Infrastructure.Database;
using portlocator.Infrastructure.Seeder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portlocator.Infrastructure
{
    public static class RegisterInjection
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configs)
        {
            string connString = configs.GetConnectionString("Database")!;

            services.AddDbContext<AppDbContext>((provider, options) =>
            {
                options.UseNpgsql(connString).UseSnakeCaseNamingConvention();
            });

            //services.AddScoped<AppDbContext>();
            //services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());

            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<DatabaseSeeder>();

            return services;
        }
    }
}
