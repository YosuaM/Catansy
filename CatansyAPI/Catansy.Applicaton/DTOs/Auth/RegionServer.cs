namespace Catansy.Applicaton.DTOs.Auth
{
    public class RegionDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class ServerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid RegionId { get; set; }
    }

    public class RegionWithServersDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<ServerDto> Servers { get; set; } = new();
    }
}
