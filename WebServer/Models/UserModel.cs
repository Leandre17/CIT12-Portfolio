namespace WebServer.Models
{
    public class UserDto
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Link { get; set; }
    }
    public class CreateUserDto
    {
        public string? Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? Role { get; set; }
        public string? salt { get; set; }
    }
    public class UpdateUserDto
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
    }
    public class LoginUserDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

}