using DataLayer;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using WebServer.Models;

namespace WebApi.Controllers;
[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IDataService _dataService;

    public UserController(IDataService dataService)
    {
        _dataService = dataService;
    }

    [HttpGet("{id}")]
    public IActionResult GetUserById(int id)
    {
        var user = _dataService.GetUserById(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost]
    public IActionResult CreateUser([FromBody] CreateUserDto createUserDto)
    {
        if (createUserDto == null)
        {
            return BadRequest();
        }

        var user = createUserDto.Adapt<User>();
        var createdUser = _dataService.CreateUser(user);

        if (createdUser == null)
        {
            return StatusCode(500, "A problem happened while handling your request.");
        }

        var userDto = createdUser.Adapt<UserDto>();
        return CreatedAtAction(nameof(GetUserById), new { id = createdUser.UserId }, userDto);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
    {
        if (updateUserDto == null)
        {
            return BadRequest();
        }

        var existingUser = _dataService.GetUserById(id);
        if (existingUser == null)
        {
            return NotFound();
        }

        updateUserDto.Adapt(existingUser);
        var IsUpdatedUser = _dataService.UpdateUser(existingUser);
        var updatedUser = _dataService.GetUserById(id);
        if (IsUpdatedUser)
        {
            return StatusCode(500, "A problem happened while handling your request.");
        }

        var userDto = updatedUser.Adapt<UserDto>();
        return Ok(userDto);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
    {
        var existingUser = _dataService.GetUserById(id);
        if (existingUser == null)
        {
            return NotFound();
        }

        var result = _dataService.DeleteUser(id);
        if (!result)
        {
            return StatusCode(500, "A problem happened while handling your request.");
        }

        return NoContent();
    }
    [HttpPost("login")]
    public IActionResult LoginUser([FromBody] LoginUserDto loginUserDto)
    {
        if (loginUserDto == null)
        {
            return BadRequest();
        }

        var user = _dataService.AuthenticateUser(loginUserDto.Email, loginUserDto.Password);
        if (user == null)
        {
            return Unauthorized();
        }

        var userDto = user.Adapt<UserDto>();
        return Ok(userDto);
    }
    [HttpPost("logout")]
    public IActionResult LogoutUser()
    {
        // Assuming you have some session or token management to handle logout
        // This is a placeholder for the actual logout logic
        // For example, you might invalidate a JWT token or clear a session

        // Placeholder logic for logout
        bool isLoggedOut = _dataService.LogoutUser();

        if (!isLoggedOut)
        {
            return StatusCode(500, "A problem happened while handling your request.");
        }

        return NoContent();
    }
}