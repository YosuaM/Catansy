using Catansy.Applicaton.Services.Implementation.Auth;
using Catansy.Applicaton.Services.Implementation.Game;
using Catansy.Applicaton.Services.Interfaces.Auth;
using Catansy.Applicaton.Services.Interfaces.Game;
using Microsoft.Extensions.DependencyInjection;

namespace Catansy.Applicaton
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICharacterService, CharacterService>();
            services.AddScoped<IIdleProgressService, IdleProgressService>();
            services.AddScoped<IRegionService, RegionService>();

            return services;
        }
    }
}
