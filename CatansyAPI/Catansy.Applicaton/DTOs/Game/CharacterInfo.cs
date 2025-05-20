namespace Catansy.Applicaton.DTOs.Game
{
    public class CharacterCreateRequest
    {
        public string Name { get; set; } = string.Empty;
        public Guid ServerId { get; set; }
    }

    public class CharacterDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid ServerId { get; set; }
        public string ServerName { get; set; } = string.Empty;
    }
}
