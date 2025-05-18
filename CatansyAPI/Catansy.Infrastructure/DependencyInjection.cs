using Catansy.Applicaton.Repositories.Interfaces.Auth;
using Catansy.Infrastructure.Persistence;
using Catansy.Infrastructure.Repositories.Implementation.Auth;
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

            return services;
        }
    }
}
