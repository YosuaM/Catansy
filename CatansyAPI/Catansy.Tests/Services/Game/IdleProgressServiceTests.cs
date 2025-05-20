using Catansy.Applicaton.Repositories.Interfaces.Game;
using Catansy.Applicaton.Services.Implementation.Game;
using Catansy.Domain.Game;
using FluentAssertions;
using Moq;
using Xunit;

namespace Catansy.Tests.Services.Game
{
    public class IdleProgressServiceTests
    {
        private readonly Mock<ICharacterRepository> _characterRepoMock = new();
        private readonly IdleProgressService _sut;

        public IdleProgressServiceTests()
        {
            _sut = new IdleProgressService(_characterRepoMock.Object);
        }


        #region Claim Rewards
        [Fact]
        public async Task ClaimRewards_Should_Throw_If_Character_Not_Found()
        {
            _characterRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Character?)null);

            Func<Task> act = async () =>
                await _sut.ClaimRewardsAsync(Guid.NewGuid(), Guid.NewGuid());

            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Character not found.");
        }

        [Fact]
        public async Task ClaimRewards_Should_Throw_If_Character_Not_Owned()
        {
            var character = new Character
            {
                Id = Guid.NewGuid(),
                AccountId = Guid.NewGuid(),
                LastCollectedAt = DateTime.UtcNow.AddMinutes(-10)
            };

            _characterRepoMock.Setup(r => r.GetByIdAsync(character.Id))
                .ReturnsAsync(character);

            var otherAccount = Guid.NewGuid();

            Func<Task> act = async () =>
                await _sut.ClaimRewardsAsync(otherAccount, character.Id);

            await act.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("You do not own this character.");
        }

        [Fact]
        public async Task ClaimRewards_Should_Throw_If_No_Time_Has_Elapsed()
        {
            var accountId = Guid.NewGuid();
            var character = new Character
            {
                Id = Guid.NewGuid(),
                AccountId = accountId,
                LastCollectedAt = DateTime.UtcNow
            };

            _characterRepoMock.Setup(r => r.GetByIdAsync(character.Id))
                .ReturnsAsync(character);

            Func<Task> act = async () =>
                await _sut.ClaimRewardsAsync(accountId, character.Id);

            await act.Should().ThrowAsync<Exception>()
                .WithMessage("No rewards available yet.");
        }

        [Fact]
        public async Task ClaimRewards_Should_Calculate_Correct_Gold_And_XP()
        {
            var accountId = Guid.NewGuid();
            var character = new Character
            {
                Id = Guid.NewGuid(),
                AccountId = accountId,
                Level = 1,
                Experience = 0,
                Gold = 0,
                LastCollectedAt = DateTime.UtcNow.AddMinutes(-5)
            };

            _characterRepoMock.Setup(r => r.GetByIdAsync(character.Id))
                .ReturnsAsync(character);

            _characterRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Character>()))
                .Returns(Task.CompletedTask);

            var result = await _sut.ClaimRewardsAsync(accountId, character.Id);

            result.GoldEarned.Should().Be(50);
            result.ExperienceEarned.Should().Be(25);
            result.TotalGold.Should().Be(50);
            result.TotalExperience.Should().Be(25);
            result.Level.Should().Be(1);
        }

        [Fact]
        public async Task ClaimRewards_Should_Level_Up_When_XP_Reaches_Threshold()
        {
            var accountId = Guid.NewGuid();
            var character = new Character
            {
                Id = Guid.NewGuid(),
                AccountId = accountId,
                Level = 1,
                Experience = 90,
                LastCollectedAt = DateTime.UtcNow.AddMinutes(-2) // +10 XP → total 100
            };

            _characterRepoMock.Setup(r => r.GetByIdAsync(character.Id))
                .ReturnsAsync(character);

            _characterRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Character>()))
                .Returns(Task.CompletedTask);

            var result = await _sut.ClaimRewardsAsync(accountId, character.Id);

            result.Level.Should().Be(2);
            result.TotalExperience.Should().Be(0); // 100 XP requerido para subir
        }
        #endregion
    }
}
