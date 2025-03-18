using CSharpFunctionalExtensions;
using AirportSystem.Application.Models;
using AirportSystem.Application.Interfaces;

namespace AirportSystem.Infrastructure.Services
{
    public class CalculateDistanceService : ICalculateDistanceService
    {
        public async Task<IResult<double>> CalculateDistance(Location firstData, Location secondData)
        {
            const double EarthRadiusMiles = 3958.8; // Radius of the Earth in miles

            double dLat = ToRadians(secondData.Lat - firstData.Lat);
            double dLon = ToRadians(secondData.Lon - firstData.Lon);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRadians(firstData.Lat)) * Math.Cos(ToRadians(secondData.Lat)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double response = EarthRadiusMiles * c;
            return Result.Success(response);
        }
        private static double ToRadians(double angleInDegrees) => angleInDegrees * Math.PI / 180;
    }
}