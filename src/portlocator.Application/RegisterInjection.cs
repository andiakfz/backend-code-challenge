using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using portlocator.Application.Abstraction.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portlocator.Application
{
    public static class RegisterInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(RegisterInjection).Assembly);

                config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            });

            services.AddValidatorsFromAssembly(typeof(RegisterInjection).Assembly, includeInternalTypes: true);

            return services;
        }
    }
}
