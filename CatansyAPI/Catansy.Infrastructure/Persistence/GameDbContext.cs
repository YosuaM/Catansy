using Catansy.Domain.Auth;
using Catansy.Domain.Game;
using Microsoft.EntityFrameworkCore;

namespace Catansy.Infrastructure.Persistence
{
    public class GameDbContext : DbContext
    {
        public GameDbContext(DbContextOptions<GameDbContext> options)
            : base(options)
        {
        }

        #region Entites
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Character> Characters => Set<Character>();
        public DbSet<Region> Regions => Set<Region>();
        public DbSet<Server> Servers => Set<Server>();
        #endregion

        #region Methods
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Region → Server (1:N)
            modelBuilder.Entity<Region>()
                .HasMany(r => r.Servers)
                .WithOne(s => s.Region)
                .HasForeignKey(s => s.RegionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Server → Character (1:N)
            modelBuilder.Entity<Server>()
                .HasMany(s => s.Characters)
                .WithOne(c => c.Server)
                .HasForeignKey(c => c.ServerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Account → Character (1:N)
            modelBuilder.Entity<Account>()
                .HasMany(a => a.Characters)
                .WithOne(c => c.Account)
                .HasForeignKey(c => c.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            // Restrinction: an account can only have one character per server
            modelBuilder.Entity<Character>()
                .HasIndex(c => new { c.AccountId, c.ServerId })
                .IsUnique();
        }
        #endregion
    }
}
