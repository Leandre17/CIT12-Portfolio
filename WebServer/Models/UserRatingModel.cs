namespace WebServer.Models
{
    public class UserRatingDTO
    {
        public string? MovieId { get; set; }
        public int Rating { get; set; }
        public string? Link { get; internal set; }
    }
}