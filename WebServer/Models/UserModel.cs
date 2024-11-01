namespace WebServer.Models
{
    public class UserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
    }
    public class CreateUserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class UpdateUserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
    }
    public class LoginUserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

}