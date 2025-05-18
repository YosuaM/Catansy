using Catansy.Applicaton.Repositories.Interfaces.Game;
using Catansy.Domain.Game;
using Catansy.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Catansy.Infrastructure.Repositories.Implementation.Game
{
    public class CharacterRepository : ICharacterRepository
    {
        #region Attributes
        private readonly GameDbContext _context;
        #endregion


        #region Constructor
        public CharacterRepository(GameDbContext context)
        {
            _context = context;
        }
        #endregion


        #region Public methods
        public async Task<List<Character>> GetByAccountIdAsync(Guid accountId)
        {
            return await _context.Characters
                .Include(c => c.Server)
                .Where(c => c.AccountId == accountId)
                .ToListAsync();
        }

        public async Task<bool> ExistsForAccountInServerAsync(Guid accountId, Guid serverId)
        {
            return await _context.Characters
                .AnyAsync(c => c.AccountId == accountId && c.ServerId == serverId);
        }

        public async Task AddAsync(Character character)
        {
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();
        }

        public async Task<Character?> GetByIdAsync(Guid characterId)
        {
            return await _context.Characters
                .Include(c => c.Server)
                .FirstOrDefaultAsync(c => c.Id == characterId);
        }
        #endregion
    }
}
