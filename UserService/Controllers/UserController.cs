using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Features.Users.LoginUser;
using UserService.Features.Users.RegisterUser;

namespace UserService.Controllers;

[ApiController]
[Route("api/users")]
public class UserController(ISender sender) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand userCommand)
    {
        var user = await sender.Send(userCommand);
        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand userCommand)
    {
        var token = await sender.Send(userCommand);
        if (token == null)
        {
            return BadRequest();
        }

        return Ok(token);
    }
}