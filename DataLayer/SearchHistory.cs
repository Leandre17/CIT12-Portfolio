namespace DataLayer;
public class SearchHistory
{
    public int Id { get; set; }
    public string? SearchTerm { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
}