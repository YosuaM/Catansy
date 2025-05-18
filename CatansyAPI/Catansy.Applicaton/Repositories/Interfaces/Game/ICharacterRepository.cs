using Catansy.Domain.Game;

namespace Catansy.Applicaton.Repositories.Interfaces.Game
{
    public interface ICharacterRepository
    {
        Task<List<Character>> GetByAccountIdAsync(Guid accountId);
        Task<bool> ExistsForAccountInServerAsync(Guid accountId, Guid serverId);
        Task AddAsync(Character character);
    }
}
