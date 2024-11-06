using DataLayer;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using WebServer.Models;
using WebServer.Services;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System;


namespace WebApi.Controllers;
[ApiController]
[Route("api/users")]
[Produces("application/json")]
[Consumes("application/json")]
public class UserController : ControllerBase
{
    private readonly IDataService _dataService;
    private readonly LinkGenerator _linkGenerator;
    private readonly Hashing _hashing;
    public UserController(IDataService dataService, LinkGenerator linkGenerator, Hashing hashing)
    {
        _dataService = dataService;
        _linkGenerator = linkGenerator;
        _hashing = hashing;
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
        return Ok(userDto);
    }

    [HttpPost]
    public IActionResult CreateUser([FromBody] CreateUserDto createUserDto)
    {
        if (createUserDto == null)
        {
            return BadRequest();
        }

        var user = createUserDto.Adapt<User>();
        (user.Password, user.Salt) = _hashing.Hash(user.Password);
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
        if (!IsUpdatedUser)
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
        var user = _dataService.GetUser(loginUserDto.Email);
        if (user == null)
        {
            return BadRequest();
        }
        if (!_hashing.Verify(loginUserDto.Password, user.Password, user.Salt))
        {
            return BadRequest();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };
        var secret = "this is a secret key for authentication";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: creds
        );
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        var userDto = user.Adapt<UserDto>();
        userDto.Link = _linkGenerator.GetUriByAction(HttpContext, nameof(GetUserById), values: new { id = user.Id });
        return Ok(tokenString);
    }
}
