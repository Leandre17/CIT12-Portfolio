namespace DataLayer;
public class Title
{
    public string Id { get; set; }
    public string? Type { get; set; }
    public string? PrimaryTitle { get; set; }
    public string? OriginalTitle { get; set; }
    public bool IsAdult { get; set; }
    public int StartYear { get; set; }
    public int EndYear { get; set; }
    public int RunTimesMinutes {get; set; }
    public string? Genres { get; set; }
}

public class Movie 
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int? Year { get; set; }
    public string? Genre { get; set; }
}
