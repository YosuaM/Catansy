using Catansy.Domain.Auth;

namespace Catansy.Applicaton.Repositories.Interfaces.Auth
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsync();
        Task<List<Server>> GetServersByRegionAsync(Guid regionId);
    }
}
