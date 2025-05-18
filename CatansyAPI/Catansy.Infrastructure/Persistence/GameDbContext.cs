using Catansy.Domain.Auth;
using Microsoft.EntityFrameworkCore;

namespace Catansy.Infrastructure.Persistence
{
    public class GameDbContext : DbContext
    {
        public GameDbContext(DbContextOptions<GameDbContext> options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts => Set<Account>();
    }
}
