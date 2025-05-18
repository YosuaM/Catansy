using Catansy.Applicaton.DTOs.Auth;
using Catansy.Applicaton.Repositories.Interfaces.Auth;
using Catansy.Applicaton.Services.Interfaces.Auth;

namespace Catansy.Applicaton.Services.Implementation.Auth
{
    public class RegionService : IRegionService
    {
        #region Attributes
        private readonly IRegionRepository _regionRepository;
        #endregion


        #region Constructor
        public RegionService(IRegionRepository regionRepository)
        {
            _regionRepository = regionRepository;
        }
        #endregion


        #region Public methods
        public async Task<IEnumerable<RegionDto>> GetAllRegionsAsync()
        {
            var regions = await _regionRepository.GetAllAsync();
            return regions.Where(r => r.Enabled).Select(r => new RegionDto
            {
                Id = r.Id,
                Name = r.Name
            });
        }

        public async Task<IEnumerable<RegionWithServersDto>> GetAllRegionsWithServersAsync()
        {
            var regions = await _regionRepository.GetAllAsync();

            var result = new List<RegionWithServersDto>();

            foreach (var region in regions.Where(r => r.Enabled))
            {
                var servers = await _regionRepository.GetServersByRegionAsync(region.Id);
                result.Add(new RegionWithServersDto
                {
                    Id = region.Id,
                    Name = region.Name,
                    Servers = servers.Where(r => r.Enabled).Select(s => new ServerDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        RegionId = s.RegionId
                    }).ToList()
                });
            }

            return result;
        }

        public async Task<IEnumerable<ServerDto>> GetServersByRegionAsync(Guid regionId)
        {
            var servers = await _regionRepository.GetServersByRegionAsync(regionId);
            return servers.Where(r => r.Enabled).Select(s => new ServerDto
            {
                Id = s.Id,
                Name = s.Name,
                RegionId = s.RegionId
            });
        }
        #endregion
    }
}
