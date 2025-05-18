using Catansy.Domain.Game;
using System.ComponentModel.DataAnnotations;

namespace Catansy.Domain.Auth
{
    public class Server
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        public Guid RegionId { get; set; }
        public Region Region { get; set; } = null!;

        public bool Enabled { get; set; } = false;

        public ICollection<Character> Characters { get; set; } = new List<Character>();
    }
}
