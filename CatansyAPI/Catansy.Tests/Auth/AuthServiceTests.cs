using Catansy.Applicaton.DTOs.Auth;
using Catansy.Applicaton.Repositories.Interfaces.Auth;
using Catansy.Applicaton.Services.Implementation.Auth;
using Catansy.Domain.Auth;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Catansy.Tests.Auth
{
    public class AuthServiceTests
    {
        private readonly Mock<IAccountRepository> _accountRepoMock = new();
        private readonly IConfiguration _config;

        private readonly AuthService _sut;

        public AuthServiceTests()
        {
            var inMemorySettings = new Dictionary<string, string> {
                { "Jwt:Key", "supersecretkeysupersecretkey1234" }
            };
            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _sut = new AuthService(_accountRepoMock.Object, _config);
        }


        #region Register
        [Fact]
        public async Task RegisterAsync_Should_Throw_If_Username_Exists()
        {
            var request = new RegisterRequest { Username = "admin", Password = "pass" };

            _accountRepoMock.Setup(r => r.GetByUsernameAsync("admin"))
                .ReturnsAsync(new Account());

            Func<Task> act = async () => await _sut.RegisterAsync(request);

            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Username already exists.");
        }

        [Fact]
        public async Task RegisterAsync_Should_Create_Account_And_Return_Token()
        {
            var request = new RegisterRequest { Username = "newuser", Password = "pass" };

            _accountRepoMock.Setup(r => r.GetByUsernameAsync("newuser"))
                .ReturnsAsync((Account?)null);

            _accountRepoMock.Setup(r => r.AddAsync(It.IsAny<Account>()))
                .Returns(Task.CompletedTask);

            var result = await _sut.RegisterAsync(request);

            result.Should().NotBeNull();
            result.Token.Should().NotBeNullOrEmpty();
        }
        #endregion


        #region Login
        [Fact]
        public async Task LoginAsync_Should_Throw_If_Username_Not_Found()
        {
            var request = new LoginRequest { Username = "nouser", Password = "pass" };

            _accountRepoMock.Setup(r => r.GetByUsernameAsync("nouser"))
                .ReturnsAsync((Account?)null);

            Func<Task> act = async () => await _sut.LoginAsync(request);

            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Invalid credentials.");
        }

        [Fact]
        public async Task LoginAsync_Should_Throw_If_Password_Is_Invalid()
        {
            var account = new Account
            {
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("correctpass")
            };

            _accountRepoMock.Setup(r => r.GetByUsernameAsync("admin"))
                .ReturnsAsync(account);

            var request = new LoginRequest { Username = "admin", Password = "wrongpass" };

            Func<Task> act = async () => await _sut.LoginAsync(request);

            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Invalid credentials.");
        }

        [Fact]
        public async Task LoginAsync_Should_Return_Token_If_Valid()
        {
            var account = new Account
            {
                Id = Guid.NewGuid(),
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("pass")
            };

            _accountRepoMock.Setup(r => r.GetByUsernameAsync("admin"))
                .ReturnsAsync(account);

            var request = new LoginRequest { Username = "admin", Password = "pass" };

            var result = await _sut.LoginAsync(request);

            result.Should().NotBeNull();
            result.Token.Should().NotBeNullOrEmpty();
        }
        #endregion
    }
}
