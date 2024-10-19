using AirportSystem.Application.Models;
using CSharpFunctionalExtensions;

namespace AirportSystem.Application.Interfaces
{
    public interface ICalculateDistanceService
    {
        Task<IResult<double>> CalculateDistance(Location firstData, Location secondData);
    }
}