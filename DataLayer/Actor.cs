namespace DataLayer
{
    public class Actor
    {
        public required string NConst { get; set; }
        public string? PrimaryName { get; set; }
        public string? BirthYear { get; set; }
        public string? DeathYear { get; set; }
        public string? PrimaryProfession { get; set; }
        public string? KnownForTitles { get; set; }
    }
}