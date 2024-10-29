namespace DataLayer;
public class Title
{
    public int Id { get; set; }
    public string Type { get; set; }
    public string PrimaryTitle { get; set; }
    public string OriginalTitle { get; set; }
    public bool IsAdult { get; set; }
    public int StartYear { get; set; }
    public int EndYear { get; set; }
    public int RunTimesMinutes {get; set; }
    public string Genres { get; set; }
}