using Catansy.Domain.Auth;

namespace Catansy.Applicaton.Repositories.Interfaces.Auth
{
    public interface IServerRepository
    {
        Task<Server?> GetByIdAsync(Guid id);
    }
}
