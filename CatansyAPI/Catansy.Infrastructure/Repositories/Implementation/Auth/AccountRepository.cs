using Catansy.Applicaton.Repositories.Interfaces.Auth;
using Catansy.Domain.Auth;
using Catansy.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Catansy.Infrastructure.Repositories.Implementation.Auth
{
    public class AccountRepository : IAccountRepository
    {
        private readonly GameDbContext _context;

        public AccountRepository(GameDbContext context)
        {
            _context = context;
        }

        public async Task<Account?> GetByUsernameAsync(string username)
        {
            return await _context.Accounts
                .FirstOrDefaultAsync(p => p.Username.ToLower() == username.ToLower());
        }

        public async Task AddAsync(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
        }
    }
}
