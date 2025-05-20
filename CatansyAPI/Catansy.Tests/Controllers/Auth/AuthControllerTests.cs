using Catansy.API.Controllers;
using Catansy.Applicaton.DTOs.Auth;
using Catansy.Applicaton.Services.Interfaces.Auth;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Catansy.Tests.Controllers.Auth
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _authServiceMock = new();
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _controller = new AuthController(_authServiceMock.Object);
        }


        #region Register
        [Fact]
        public async Task Register_Should_Return_Token_If_Successful()
        {
            var request = new RegisterRequest { Username = "user", Password = "pass" };
            var response = new AuthResponse { Token = "valid-token" };

            _authServiceMock.Setup(s => s.RegisterAsync(request))
                .ReturnsAsync(response);

            var result = await _controller.Register(request);

            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            (okResult!.Value as AuthResponse)!.Token.Should().Be("valid-token");
        }

        [Fact]
        public async Task Register_Should_Return_400_If_Exception()
        {
            var request = new RegisterRequest { Username = "user", Password = "pass" };

            _authServiceMock.Setup(s => s.RegisterAsync(request))
                .ThrowsAsync(new Exception("Username already exists."));

            var result = await _controller.Register(request);

            var badRequest = result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest!.Value.Should().BeEquivalentTo(new { error = "Username already exists." });
        }
        #endregion


        #region Login
        [Fact]
        public async Task Login_Should_Return_Token_If_Successful()
        {
            var request = new LoginRequest { Username = "user", Password = "pass" };
            var response = new AuthResponse { Token = "valid-token" };

            _authServiceMock.Setup(s => s.LoginAsync(request))
                .ReturnsAsync(response);

            var result = await _controller.Login(request);

            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            (okResult!.Value as AuthResponse)!.Token.Should().Be("valid-token");
        }

        [Fact]
        public async Task Login_Should_Return_401_If_Exception()
        {
            var request = new LoginRequest { Username = "user", Password = "wrongpass" };

            _authServiceMock.Setup(s => s.LoginAsync(request))
                .ThrowsAsync(new Exception("Invalid credentials."));

            var result = await _controller.Login(request);

            var unauthorized = result as UnauthorizedObjectResult;
            unauthorized.Should().NotBeNull();
            unauthorized!.Value.Should().BeEquivalentTo(new { error = "Invalid credentials." });
        }
        #endregion
    }
}
