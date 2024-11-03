namespace WebServer.Models
{
    public class BookmarkDto
    {
        public int Id { get; set; }
        public string? Url { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Link { get; set; }
    }
    public class CreateBookmarkDto
    {
        public string? Url { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
    public class UpdateBookmarkDto
    {
        public string? Url { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
    }

}