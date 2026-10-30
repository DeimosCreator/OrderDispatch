using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace KitchenService.Controllers;

public class BaseController : ControllerBase
{
    protected string GetUserId() => User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
}