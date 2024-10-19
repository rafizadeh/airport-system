using Carter;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using AirportSystem.Application.Behaviour;
using Microsoft.Extensions.DependencyInjection;
using ApiRequestServicePackage.Dependencies;

namespace AirportSystem.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(c =>
            {
                c.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                c.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            });
            services.AddApiRequestService(options =>
            {
                options.BaseAddress = new Uri(configuration["AirportDataProvider"]);
            });
            services.AddCarter();
            return services;
        }
    }
}