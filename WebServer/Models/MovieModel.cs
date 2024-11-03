namespace WebServer.Models
{
    public class MovieDto
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Year { get; set; }
        public string? Genre { get; set; }
        public string? Link { get; set; }
    }
}