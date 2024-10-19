using ApiRequestServicePackage;
using CSharpFunctionalExtensions;
using AirportSystem.Application.Models;
using AirportSystem.Application.Interfaces;

namespace AirportSystem.Infrastructure.Services
{
    public class AirportDataService : IAirportDataService
    {
        private readonly IApiRequestService _apiRequestService;
        public AirportDataService(IApiRequestService apiRequestService) => _apiRequestService = apiRequestService;
        public async Task<IResult<IATAData>> Get(string IATAData)
        {
            var response = await _apiRequestService.GetAsync<IResult<IATAData>>("", options => options.BaseAddress = new Uri(Path.Combine(options.BaseAddress.AbsoluteUri, IATAData)));
            if(response.IsFailure || response is null)
            {
                return Result.Failure<IATAData>("Issue occcured in IATA data provider.");
            }
            return response;
        }
    }
}