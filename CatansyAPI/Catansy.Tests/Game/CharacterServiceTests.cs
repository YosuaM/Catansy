using Catansy.Applicaton.DTOs.Auth;
using Catansy.Applicaton.Repositories.Interfaces.Auth;
using Catansy.Applicaton.Repositories.Interfaces.Game;
using Catansy.Applicaton.Services.Implementation.Game;
using Catansy.Domain.Auth;
using Catansy.Domain.Game;
using FluentAssertions;
using Moq;
using Xunit;

namespace Catansy.Tests.Game
{
    public class CharacterServiceTests
    {
        private readonly Mock<ICharacterRepository> _characterRepoMock = new();
        private readonly Mock<IServerRepository> _serverRepoMock = new();

        private readonly CharacterService _sut;

        public CharacterServiceTests()
        {
            _sut = new CharacterService(_characterRepoMock.Object, _serverRepoMock.Object);
        }


        #region Create Character
        [Fact]
        public async Task CreateCharacterAsync_Should_Throw_If_Character_Already_Exists_In_Server()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var serverId = Guid.NewGuid();

            _characterRepoMock.Setup(r => r.ExistsForAccountInServerAsync(accountId, serverId))
                              .ReturnsAsync(true);

            var request = new CharacterCreateRequest
            {
                Name = "MyHero",
                ServerId = serverId
            };

            // Act
            Func<Task> act = async () => await _sut.CreateCharacterAsync(accountId, request);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Ya tienes un personaje en ese servidor.");
        }

        [Fact]
        public async Task CreateCharacterAsync_Should_Throw_If_Server_Not_Found()
        {
            var accountId = Guid.NewGuid();
            var request = new CharacterCreateRequest
            {
                Name = "Hero",
                ServerId = Guid.NewGuid()
            };

            _characterRepoMock.Setup(r => r.ExistsForAccountInServerAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                              .ReturnsAsync(false);

            _serverRepoMock.Setup(r => r.GetByIdAsync(request.ServerId))
                           .ReturnsAsync((Server?)null);

            Func<Task> act = async () => await _sut.CreateCharacterAsync(accountId, request);

            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Servidor no encontrado.");
        }

        [Fact]
        public async Task CreateCharacterAsync_Should_Create_Character_When_Valid()
        {
            var accountId = Guid.NewGuid();
            var serverId = Guid.NewGuid();
            var server = new Server { Id = serverId, Name = "TestServer" };

            _characterRepoMock.Setup(r => r.ExistsForAccountInServerAsync(accountId, serverId))
                              .ReturnsAsync(false);

            _serverRepoMock.Setup(r => r.GetByIdAsync(serverId))
                           .ReturnsAsync(server);

            _characterRepoMock.Setup(r => r.AddAsync(It.IsAny<Character>()))
                              .Returns(Task.CompletedTask);

            var request = new CharacterCreateRequest
            {
                Name = "Hero",
                ServerId = serverId
            };

            var result = await _sut.CreateCharacterAsync(accountId, request);

            result.Should().NotBeNull();
            result.Name.Should().Be("Hero");
            result.ServerId.Should().Be(serverId);
            result.ServerName.Should().Be("TestServer");
        }
        #endregion


        #region Get Character
        [Fact]
        public async Task GetCharactersForAccountAsync_Should_Return_Characters()
        {
            var accountId = Guid.NewGuid();
            var characters = new List<Character>
            {
                new() {
                    Id = Guid.NewGuid(),
                    Name = "Hero",
                    AccountId = accountId,
                    Server = new Server { Name = "S1" }
                }
            };

            _characterRepoMock.Setup(r => r.GetByAccountIdAsync(accountId))
                              .ReturnsAsync(characters);

            var result = await _sut.GetCharactersForAccountAsync(accountId);

            result.Should().HaveCount(1);
            result.First().Name.Should().Be("Hero");
        }
        #endregion



    }
}
