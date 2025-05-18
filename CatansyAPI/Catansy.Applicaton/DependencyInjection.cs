using Catansy.Applicaton.Services.Implementation.Auth;
using Catansy.Applicaton.Services.Interfaces.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace Catansy.Applicaton
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRegionService, RegionService>();

            return services;
        }
    }
}
