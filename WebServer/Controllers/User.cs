using DataLayer;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using WebServer.Models;

namespace WebApi.Controllers;
[ApiController]
[Route("api/users")]
[Produces("application/json")]
[Consumes("application/json")]
public class UserController : ControllerBase
{
    private readonly IDataService _dataService;
    private readonly LinkGenerator _linkGenerator;
    public UserController(IDataService dataService, LinkGenerator linkGenerator)
    {
        _dataService = dataService;
        _linkGenerator = linkGenerator;
    }

    [HttpGet]
    [HttpGet("{id}")]
    public IActionResult GetUserById(int id = 1)
    {
        var user = _dataService.GetUserById(id);
        if (user == null)
        {
            return NotFound();
        }
        var userDto = user.Adapt<UserDto>();
        userDto.Link = _linkGenerator.GetUriByAction(HttpContext, nameof(GetUserById), values: new { id = id });
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
        return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, userDto);
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
        if (IsUpdatedUser)
        {
            return StatusCode(500, "A problem happened while handling your request.");
        }
        return Ok();
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

        return Ok();
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
        userDto.Link = _linkGenerator.GetUriByAction(HttpContext, nameof(GetUserById), values: new { id = user.Id });
        return Ok(userDto);
    }

    [HttpPost("logout")]
    public IActionResult LogoutUser()
    {
        // not implemented in the data service
        bool isLoggedOut = _dataService.LogoutUser(1);

        if (!isLoggedOut)
        {
            return StatusCode(500, "A problem happened while handling your request.");
        }

        return Ok();
    }
}