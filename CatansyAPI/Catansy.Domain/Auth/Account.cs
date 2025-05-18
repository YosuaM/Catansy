using System.ComponentModel.DataAnnotations;

namespace Catansy.Domain.Auth
{
    public class Account
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? PasswordHash { get; set; } = null;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
