using Microsoft.Extensions.DependencyInjection;

namespace ApiRequestServicePackage.Dependencies
{
    public static class DependencyInjection
    {
        public static void AddApiRequestService(this IServiceCollection services, Action<ApiRequestServiceOptions> action = default)
        {
            ApiRequestServiceOptions apiRequestServiceOptions = new();

            action?.Invoke(apiRequestServiceOptions);

            services.AddScoped<IApiRequestService, ApiRequestService>(sp => new ApiRequestService(apiRequestServiceOptions));
        }
    }
}