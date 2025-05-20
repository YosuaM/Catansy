using Catansy.Applicaton.DTOs.Game;

namespace Catansy.Applicaton.Services.Interfaces.Game
{
    public interface IIdleProgressService
    {
        Task<IdleRewardResult> ClaimRewardsAsync(Guid accountId, Guid characterId);
    }
}
