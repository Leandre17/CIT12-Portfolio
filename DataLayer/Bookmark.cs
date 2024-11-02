namespace DataLayer;
public class Bookmark
{
    public int Id { get; set; }
    public string? ItemId { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
}