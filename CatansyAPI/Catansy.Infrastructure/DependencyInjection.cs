using Microsoft.Extensions.DependencyInjection;

namespace Catansy.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Aquí agregarás EF Core, repositorios, autenticación, SignalR, etc.
            // services.AddDbContext<GameDbContext>(...);
            // services.AddScoped<IPlayerRepository, PlayerRepository>();

            return services;
        }
    }
}
