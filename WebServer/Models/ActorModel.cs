namespace WebServer.Models
{
    public class ActorDto
    {
        public string NConst { get; set; } = string.Empty;
        public string? PrimaryName { get; set; }
        public string? BirthYear { get; set; }
        public string? DeathYear { get; set; }
        public string? Link { get; set; }
    }
}