using AirportSystem.Application.Interfaces;
using AirportSystem.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AirportSystem.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfraStructure(this IServiceCollection services)
        {
            services.AddTransient<ICalculateDistanceService, CalculateDistanceService>();
            services.AddTransient<IAirportDataService, AirportDataService>();
            return services;
        }
    }
}