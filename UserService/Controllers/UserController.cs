using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Features.Users.AssignRoleUser;
using UserService.Features.Users.GetUser;
using UserService.Features.Users.LoginUser;
using UserService.Features.Users.RegisterUser;
using UserService.Models.Enums;

namespace UserService.Controllers;

[ApiController]
[Route("api/users")]
public class UserController(ISender sender) : BaseController
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand userCommand)
    {
        var user = await sender.Send(userCommand);
        if (user == null)
        {
            return BadRequest("Пользователь уже существует");
        }
        
        return CreatedAtAction(
            nameof(GetUser),
            new { id = user.Id }, 
            user);
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

    [Authorize]
    [HttpGet("{id}/get-user")]
    public async Task<IActionResult> GetUser(string id)
    {
        var userId = GetUserId();
        var user = await sender.Send(new GetUserQuery(id, userId));
        if (user == null)
        {
            return BadRequest("Пользователь не найден");
        }

        return Ok(user);
    }
    
    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUserProfile()
    {
        var userId = GetUserId();
        var user = await sender.Send(new GetUserQuery(userId, userId));
        if (user == null)
        {
            return NotFound("Пользователь не найден");
        }

        return Ok(user);
    }

    [Authorize]
    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRole([FromBody] AssignRoleUserCommand assignRoleUserCommand)
    {
        var userId = GetUserId();
        var result = await sender.Send(assignRoleUserCommand with { UserId = userId});

        return result switch
        {
            AssignRoleUserResult.NotFound =>
                NotFound("Пользователь не найден"),

            AssignRoleUserResult.CannotAssignRole =>
                Conflict("Нельзя назначить роль"),

            AssignRoleUserResult.AssignRole =>
                Ok("Роль назначена"),

            _ => StatusCode(500)
        };
    }
}