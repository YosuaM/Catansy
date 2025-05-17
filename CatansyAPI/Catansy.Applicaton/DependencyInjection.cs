using Microsoft.Extensions.DependencyInjection;

namespace Catansy.Applicaton
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Aquí registrarás servicios de dominio, validadores, etc.
            // services.AddScoped<IPlayerService, PlayerService>();

            return services;
        }
    }
}
