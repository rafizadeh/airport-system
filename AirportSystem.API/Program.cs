using AirportSystem.Infrastructure;
using AirportSystem.Application;
using Carter;

namespace AirportSystem.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddApplication(builder.Configuration);
            builder.Services.AddInfraStructure();
            var app = builder.Build();
            app.MapCarter();
            app.Run();
        }
    }
}