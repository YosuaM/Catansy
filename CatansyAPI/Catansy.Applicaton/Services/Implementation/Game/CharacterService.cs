using Catansy.Applicaton.DTOs.Auth;
using Catansy.Applicaton.Repositories.Interfaces.Auth;
using Catansy.Applicaton.Repositories.Interfaces.Game;
using Catansy.Applicaton.Services.Interfaces.Game;
using Catansy.Domain.Game;

namespace Catansy.Applicaton.Services.Implementation.Game
{
    public class CharacterService : ICharacterService
    {
        #region Attributes
        private readonly ICharacterRepository _characterRepo;
        private readonly IServerRepository _serverRepo;
        #endregion


        #region Constructor
        public CharacterService(ICharacterRepository characterRepo, IServerRepository serverRepo)
        {
            _characterRepo = characterRepo;
            _serverRepo = serverRepo;
        }
        #endregion


        #region Public methods
        public async Task<IEnumerable<CharacterDto>> GetCharactersForAccountAsync(Guid accountId)
        {
            var characters = await _characterRepo.GetByAccountIdAsync(accountId);
            return characters.Select(c => new CharacterDto
            {
                Id = c.Id,
                Name = c.Name,
                ServerId = c.ServerId,
                ServerName = c.Server?.Name ?? string.Empty
            });
        }

        public async Task<CharacterDto> CreateCharacterAsync(Guid accountId, CharacterCreateRequest request)
        {
            // Verify if the account already has a character in the server
            if (await _characterRepo.ExistsForAccountInServerAsync(accountId, request.ServerId))
                throw new Exception("Ya tienes un personaje en ese servidor.");

            var server = await _serverRepo.GetByIdAsync(request.ServerId)
                         ?? throw new Exception("Servidor no encontrado.");

            var character = new Character
            {
                Name = request.Name,
                AccountId = accountId,
                ServerId = request.ServerId
            };

            await _characterRepo.AddAsync(character);

            return new CharacterDto
            {
                Id = character.Id,
                Name = character.Name,
                ServerId = server.Id,
                ServerName = server.Name
            };
        }
        #endregion
    }
}
