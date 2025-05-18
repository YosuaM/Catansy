using Catansy.Domain.Auth;
using System.ComponentModel.DataAnnotations;

namespace Catansy.Domain.Game
{
    public class Character
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        public Guid AccountId { get; set; }
        public Account Account { get; set; } = null!;

        public Guid ServerId { get; set; }
        public Server Server { get; set; } = null!;
    }

}
