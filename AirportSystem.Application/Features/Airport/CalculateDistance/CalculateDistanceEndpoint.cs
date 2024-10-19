using Carter;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;

namespace AirportSystem.Application.Features.Airport.CalculateDistance
{
    public class CalculateDistanceEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/airport/distance", async ([FromBody]CalculateDistanceRequest request, ISender sender) =>
            {
                var response = await sender.Send(new CalculateDistanceQuery(request.FirstIATAData, request.SecondIATAData));
                if (!response.IsSuccess)
                    return Results.Problem(response.Error);    
                return Results.Ok(response.Value);
            });
        }
    }
}