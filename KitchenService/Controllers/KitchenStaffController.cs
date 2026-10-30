using KitchenService.Features.KitchenStaff.AssignKitchenStaff;
using KitchenService.Models.Entities.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KitchenService.Controllers;

[ApiController]
[Route("api/kitchen_staff")]
[Authorize]
public class KitchenStaffController(ISender sender) : BaseController
{
    [HttpPost("assign")]
    public async Task<IActionResult> AssignKitchenStaffOrder(
        [FromBody] AssignKitchenStaffCommand assignKitchenStaffCommand)
    {
        var userId = GetUserId();
        var result = await sender.Send(assignKitchenStaffCommand with { UserId = userId });
        return result switch
        {
            AssignKitchenStaffStatus.KitchenNotFound => BadRequest("Кухня не найдена"),
            AssignKitchenStaffStatus.UserNotFound => BadRequest("Пользователь не найден"),
            AssignKitchenStaffStatus.CannotAssignKitchenStaff => Conflict("Не удалось назначить кухню"),
            AssignKitchenStaffStatus.AssignKitchenStaff => Ok(),
            _ => BadRequest()
        };
    }
}