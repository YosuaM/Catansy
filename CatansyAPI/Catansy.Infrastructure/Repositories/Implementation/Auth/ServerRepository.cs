using Catansy.Applicaton.Repositories.Interfaces.Auth;
using Catansy.Domain.Auth;
using Catansy.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Catansy.Infrastructure.Repositories.Implementation.Auth
{
    public class ServerRepository : IServerRepository
    {
        #region Attributes
        private readonly GameDbContext _context;
        #endregion


        #region Constructor
        public ServerRepository(GameDbContext context)
        {
            _context = context;
        }
        #endregion


        #region Public methods
        public async Task<Server?> GetByIdAsync(Guid id)
        {
            return await _context.Servers
                .Include(s => s.Region)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
        #endregion
    }

}
