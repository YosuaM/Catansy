﻿using Catansy.Applicaton.DTOs.Game;
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
                throw new Exception("You already have a character on this server.");

            var server = await _serverRepo.GetByIdAsync(request.ServerId)
                         ?? throw new Exception("Server not found.");

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

        public async Task<CharacterDto?> GetCharacterByIdAsync(Guid accountId, Guid characterId)
        {
            var character = await _characterRepo.GetByIdAsync(characterId);

            if (character == null || character.AccountId != accountId)
                return null;

            return new CharacterDto
            {
                Id = character.Id,
                Name = character.Name,
                ServerId = character.ServerId,
                ServerName = character.Server?.Name ?? string.Empty
            };
        }
        #endregion
    }
}
