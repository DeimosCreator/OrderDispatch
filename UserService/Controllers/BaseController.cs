using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace UserService.Controllers;

public class BaseController : ControllerBase
{
    protected string GetUserId() => User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
}