using AirportSystem.Application.Interfaces;
using AirportSystem.Application.Models;
using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;

namespace AirportSystem.Application.Features.Airport.CalculateDistance
{
    public record CalculateDistanceQuery : IRequest<IResult<CalculateDistanceResponse>>
    {
        public string FirstIATAData { get; set; }
        public string SecondIATAData { get; set; }
        public CalculateDistanceQuery(string firstIATAData, string secondIATAData)
        {
            FirstIATAData = firstIATAData;
            SecondIATAData = secondIATAData;
        }
        public class CalculateDistanceQueryValidator : AbstractValidator<CalculateDistanceQuery>
        {
            public CalculateDistanceQueryValidator()
            {
                RuleFor(x => x.FirstIATAData).NotEqual(x => x.SecondIATAData).WithMessage("IATA data must not be equal");

                RuleFor(x => x.FirstIATAData).NotNull().NotEmpty().WithMessage("First IATA data must not be null.");
                RuleFor(x => x.SecondIATAData).NotNull().NotEmpty().WithMessage("Second IATA data must not be null.");

                RuleFor(x => x.FirstIATAData).Length(3).WithMessage("First IATA data must contain only 3 characters.");
                RuleFor(x => x.SecondIATAData).Length(3).WithMessage("Second IATA data must contain only 3 characters.");

                RuleFor(x => x.FirstIATAData).Must(x => x == x.ToUpper()).WithMessage("First IATA data must be in upper case.");
                RuleFor(x => x.SecondIATAData).Must(x => x == x.ToUpper()).WithMessage("Second IATA data must be in upper case.");
            }
        }
        public class CalculateDistanceQueryHandler(ICalculateDistanceService calculateDistanceService, IAirportDataService airportDataService) : IRequestHandler<CalculateDistanceQuery, IResult<CalculateDistanceResponse>>
        {
            public async Task<IResult<CalculateDistanceResponse>> Handle(CalculateDistanceQuery request, CancellationToken cancellationToken)
            {
                var firstAirportData = await airportDataService.Get(request.FirstIATAData);
                var secondAirportData = await airportDataService.Get(request.SecondIATAData);

                if(!firstAirportData.IsSuccess || !secondAirportData.IsSuccess)
                {
                    string errorMessage = firstAirportData.Error ?? secondAirportData.Error;
                    return Result.Failure<CalculateDistanceResponse>(errorMessage);
                }

                Location firstData = new()
                {
                    Lat = firstAirportData.Value.Location.Lat,
                    Lon = firstAirportData.Value.Location.Lon
                };
                Location secondData = new()
                {
                    Lat = secondAirportData.Value.Location.Lat,
                    Lon = secondAirportData.Value.Location.Lon
                };

                var response = await calculateDistanceService.CalculateDistance(firstData, secondData);
                return Result.Success(new CalculateDistanceResponse()
                {
                    Distance = response.Value
                });
            }
        }
    }
}