namespace DataLayer;
public class User
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}
public class UserDTO
{
    public string? Name { get; set; }
    public string? Email { get; set; }
}

public class UserRating
{
    public int RatingId { get; set; }
    public int UserId { get; set; }
    public string? MovieId { get; set; }
    public int Rating { get; set; }
}
