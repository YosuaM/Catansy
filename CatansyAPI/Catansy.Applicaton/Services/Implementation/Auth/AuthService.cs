using Catansy.Applicaton.DTOs.Auth;
using Catansy.Applicaton.Repositories.Interfaces.Auth;
using Catansy.Applicaton.Services.Interfaces.Auth;
using Catansy.Domain.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Catansy.Applicaton.Services.Implementation.Auth
{
    public class AuthService : IAuthService
    {
        #region Attributes
        private readonly IAccountRepository _accountRepository;
        private readonly IConfiguration _configuration;
        #endregion


        #region Constructor
        public AuthService(IAccountRepository accountRepository, IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _configuration = configuration;
        }
        #endregion


        #region  Public methods
        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var existinAccount = await _accountRepository.GetByUsernameAsync(request.Username);
            if (existinAccount != null)
                throw new Exception("Username already exists.");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var account = new Account
            {
                Username = request.Username,
                PasswordHash = passwordHash
            };

            await _accountRepository.AddAsync(account);

            var token = GenerateJwtToken(account);
            return new AuthResponse { Token = token };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var player = await _accountRepository.GetByUsernameAsync(request.Username);
            if (player == null || !BCrypt.Net.BCrypt.Verify(request.Password, player.PasswordHash))
                throw new Exception("Invalid credentials.");

            var token = GenerateJwtToken(player);
            return new AuthResponse { Token = token };
        }
        #endregion


        #region Private methods
        private string GenerateJwtToken(Account account)
        {
            var jwtKey = _configuration["Jwt:Key"] ?? throw new Exception("JWT key not configured.");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Name, account.Username)
            };

            var token = new JwtSecurityToken(
                expires: DateTime.UtcNow.AddDays(30),
                signingCredentials: credentials,
                claims: claims
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion
    }
}
