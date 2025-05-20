using Catansy.Applicaton.DTOs.Game;
using Catansy.Applicaton.Repositories.Interfaces.Game;
using Catansy.Applicaton.Services.Interfaces.Game;

namespace Catansy.Applicaton.Services.Implementation.Game
{
    public class IdleProgressService : IIdleProgressService
    {
        #region Attributes
        private readonly ICharacterRepository _characterRepo;
        #endregion


        #region Constructor
        public IdleProgressService(ICharacterRepository characterRepo)
        {
            _characterRepo = characterRepo;
        }
        #endregion


        #region Public methods
        public async Task<IdleRewardResult> ClaimRewardsAsync(Guid accountId, Guid characterId)
        {
            var character = await _characterRepo.GetByIdAsync(characterId)
                ?? throw new Exception("Character not found.");

            if (character.AccountId != accountId)
                throw new UnauthorizedAccessException("You do not own this character.");

            var now = DateTime.UtcNow;
            var elapsedMinutes = (int)(now - character.LastCollectedAt).TotalMinutes;

            if (elapsedMinutes <= 0)
                throw new Exception("No rewards available yet.");

            int goldEarned = elapsedMinutes * 10;
            int xpEarned = elapsedMinutes * 5;

            character.Gold += goldEarned;
            character.Experience += xpEarned;
            character.LastCollectedAt = now;

            // Simple level-up system
            while (character.Experience >= character.Level * 100)
            {
                character.Experience -= character.Level * 100;
                character.Level++;
            }

            await _characterRepo.UpdateAsync(character);

            return new IdleRewardResult
            {
                GoldEarned = goldEarned,
                ExperienceEarned = xpEarned,
                TotalGold = character.Gold,
                TotalExperience = character.Experience,
                Level = character.Level
            };
        }
        #endregion
    }
}
