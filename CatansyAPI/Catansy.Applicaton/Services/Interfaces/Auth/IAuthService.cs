using Catansy.Applicaton.DTOs.Auth;

namespace Catansy.Applicaton.Services.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
    }
}
