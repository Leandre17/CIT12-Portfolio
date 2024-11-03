namespace WebServer.Models
{
    public class SearchHistoryDTO
    {
        public int Id { get; set; }
        public string? SearchTerm { get; set; }
        public int UserId { get; set; }
        public string? Link { get; set; }
    }
}