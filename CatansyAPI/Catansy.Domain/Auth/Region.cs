using System.ComponentModel.DataAnnotations;

namespace Catansy.Domain.Auth
{
    public class Region
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        public bool Enabled { get; set; } = false;

        public ICollection<Server> Servers { get; set; } = new List<Server>();
    }
}
