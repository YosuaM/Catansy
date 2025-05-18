using Catansy.Applicaton.DTOs.Auth;

namespace Catansy.Applicaton.Services.Interfaces.Game
{
    public interface ICharacterService
    {
        Task<IEnumerable<CharacterDto>> GetCharactersForAccountAsync(Guid accountId);
        Task<CharacterDto> CreateCharacterAsync(Guid accountId, CharacterCreateRequest request);
        Task<CharacterDto?> GetCharacterByIdAsync(Guid accountId, Guid characterId);
    }
}
