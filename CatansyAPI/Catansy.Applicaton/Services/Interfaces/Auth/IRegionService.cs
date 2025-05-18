using Catansy.Applicaton.DTOs.Auth;

namespace Catansy.Applicaton.Services.Interfaces.Auth
{
    public interface IRegionService
    {
        Task<IEnumerable<RegionDto>> GetAllRegionsAsync();
        Task<IEnumerable<RegionWithServersDto>> GetAllRegionsWithServersAsync();
        Task<IEnumerable<ServerDto>> GetServersByRegionAsync(Guid regionId);
    }
}
