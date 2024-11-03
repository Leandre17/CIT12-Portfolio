namespace WebServer.Models
{
    public class RatingDTO
    {
        public int RatingId { get; set; }
        public int UserId { get; set; }
        public string? MovieId { get; set; }
        public int Rating { get; set; }
        public string? Link { get; set; }
    }
}