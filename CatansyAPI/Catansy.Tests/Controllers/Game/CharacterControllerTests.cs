﻿using Catansy.API.Controllers;
using Catansy.Applicaton.DTOs.Game;
using Catansy.Applicaton.Services.Interfaces.Game;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Xunit;

namespace Catansy.Tests.Controllers.Game
{
    public class CharacterControllerTests
    {
        private readonly Mock<ICharacterService> _characterServiceMock = new();
        private readonly Mock<IIdleProgressService> _idleProgressServiceMock = new();

        private readonly CharacterController _controller;

        private readonly Guid _accountId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        public CharacterControllerTests()
        {
            _controller = new CharacterController(
                _characterServiceMock.Object,
                _idleProgressServiceMock.Object
            );

            var user = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, _accountId.ToString())
                    }
                )
            );

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }


        #region GetMyCharacters
        [Fact]
        public async Task GetMyCharacters_Should_Return_200_And_List()
        {
            // Arrange
            var characters = new List<CharacterDto>
            {
                new() { Id = Guid.NewGuid(), Name = "Hero1" },
                new() { Id = Guid.NewGuid(), Name = "Hero2" }
            };

            _characterServiceMock.Setup(s => s.GetCharactersForAccountAsync(_accountId))
                .ReturnsAsync(characters);

            // Act
            var result = await _controller.GetMyCharacters();

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            var value = okResult!.Value as IEnumerable<CharacterDto>;
            value.Should().HaveCount(2);
        }
        #endregion


        #region GetCharacterById
        [Fact]
        public async Task GetCharacterById_Should_Return_200_If_Found()
        {
            var characterId = Guid.NewGuid();
            var character = new CharacterDto { Id = characterId, Name = "Hero1" };

            _characterServiceMock.Setup(s => s.GetCharacterByIdAsync(_accountId, characterId))
                .ReturnsAsync(character);

            var result = await _controller.GetCharacterById(characterId);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetCharacterById_Should_Return_404_If_Not_Found()
        {
            var characterId = Guid.NewGuid();

            _characterServiceMock.Setup(s => s.GetCharacterByIdAsync(_accountId, characterId))
                .ReturnsAsync((CharacterDto?)null);

            var result = await _controller.GetCharacterById(characterId);

            result.Should().BeOfType<NotFoundResult>();
        }
        #endregion


        #region CreateCharacter
        [Fact]
        public async Task CreateCharacter_Should_Return_200_On_Success()
        {
            var request = new CharacterCreateRequest { Name = "Hero", ServerId = Guid.NewGuid() };
            var created = new CharacterDto { Id = Guid.NewGuid(), Name = "Hero" };

            _characterServiceMock.Setup(s => s.CreateCharacterAsync(_accountId, request))
                .ReturnsAsync(created);

            var result = await _controller.CreateCharacter(request);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task CreateCharacter_Should_Return_400_On_Exception()
        {
            var request = new CharacterCreateRequest { Name = "Hero", ServerId = Guid.NewGuid() };

            _characterServiceMock.Setup(s => s.CreateCharacterAsync(_accountId, request))
                .ThrowsAsync(new Exception("Ya tienes un personaje en ese servidor."));

            var result = await _controller.CreateCharacter(request);

            var badRequest = result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest!.Value.Should().BeEquivalentTo(new { error = "Ya tienes un personaje en ese servidor." });
        }
        #endregion


        #region ClaimIdleRewards
        [Fact]
        public async Task ClaimRewards_Should_Return_200_On_Success()
        {
            var characterId = Guid.NewGuid();
            var reward = new IdleRewardResult
            {
                GoldEarned = 100,
                ExperienceEarned = 50,
                TotalGold = 1000,
                TotalExperience = 500,
                Level = 3
            };

            _idleProgressServiceMock.Setup(s => s.ClaimRewardsAsync(_accountId, characterId))
                .ReturnsAsync(reward);

            var result = await _controller.ClaimIdleRewards(characterId);

            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            var value = ok!.Value as IdleRewardResult;
            value!.GoldEarned.Should().Be(100);
        }

        [Fact]
        public async Task ClaimRewards_Should_Return_401_If_Not_Owner()
        {
            var characterId = Guid.NewGuid();

            _idleProgressServiceMock.Setup(s => s.ClaimRewardsAsync(_accountId, characterId))
                .ThrowsAsync(new UnauthorizedAccessException("You do not own this character."));

            var result = await _controller.ClaimIdleRewards(characterId);

            result.Should().BeOfType<UnauthorizedObjectResult>();
        }

        [Fact]
        public async Task ClaimRewards_Should_Return_400_On_Business_Error()
        {
            var characterId = Guid.NewGuid();

            _idleProgressServiceMock.Setup(s => s.ClaimRewardsAsync(_accountId, characterId))
                .ThrowsAsync(new Exception("No rewards available yet."));

            var result = await _controller.ClaimIdleRewards(characterId);

            var badRequest = result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest!.Value.Should().BeEquivalentTo(new { error = "No rewards available yet." });
        }
        #endregion
    }
}
