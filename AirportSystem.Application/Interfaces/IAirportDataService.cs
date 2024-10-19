using AirportSystem.Application.Models;
using CSharpFunctionalExtensions;

namespace AirportSystem.Application.Interfaces
{
    public interface IAirportDataService
    {
        Task<IResult<IATAData>> Get(string IATAData);
    }
}