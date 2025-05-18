using Catansy.Domain.Auth;

namespace Catansy.Applicaton.Repositories.Interfaces.Auth
{
    public interface IAccountRepository
    {
        Task<Account?> GetByUsernameAsync(string username);
        Task AddAsync(Account player);
    }
}
