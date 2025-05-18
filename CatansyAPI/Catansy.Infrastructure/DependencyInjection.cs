using Catansy.Applicaton.Repositories.Interfaces.Auth;
using Catansy.Applicaton.Repositories.Interfaces.Game;
using Catansy.Infrastructure.Persistence;
using Catansy.Infrastructure.Repositories.Implementation.Auth;
using Catansy.Infrastructure.Repositories.Implementation.Game;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catansy.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<GameDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ICharacterRepository, CharacterRepository>();
            services.AddScoped<IRegionRepository, RegionRepository>();
            services.AddScoped<IServerRepository, ServerRepository>();

            return services;
        }
    }
}
