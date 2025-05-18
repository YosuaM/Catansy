using Catansy.Applicaton.Repositories.Interfaces.Auth;
using Catansy.Domain.Auth;
using Catansy.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Catansy.Infrastructure.Repositories.Implementation.Auth
{
    public class RegionRepository : IRegionRepository
    {
        #region Attributes
        private readonly GameDbContext _context;
        #endregion


        #region Constructor
        public RegionRepository(GameDbContext context)
        {
            _context = context;
        }
        #endregion


        #region Public methods
        public async Task<List<Region>> GetAllAsync()
        {
            return await _context.Regions.AsNoTracking().ToListAsync();
        }

        public async Task<List<Server>> GetServersByRegionAsync(Guid regionId)
        {
            return await _context.Servers
                .Where(s => s.RegionId == regionId)
                .AsNoTracking()
                .ToListAsync();
        }
        #endregion
    }
}
