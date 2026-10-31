using KitchenService.Features.KitchenStaff.AssignKitchenStaff;
using KitchenService.Features.KitchenStaff.GetKitchen;
using KitchenService.Features.KitchenStaff.GetKitchenStaff;
using KitchenService.Features.KitchenStaff.GetKitchenStaffsQuery;
using KitchenService.Models.Entities.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderDispatch.Contracts.Constants;
using OrderDispatch.Contracts.Security;

namespace KitchenService.Controllers;

[ApiController]
[Route("api/kitchen_staff")]
public class KitchenStaffController(ISender sender) : BaseController
{
    [Authorize]
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
    
    [Authorize]
    [HttpGet("my-kitchen")]
    public async Task<IActionResult> GetMyKitchen()
    {
        var userId = GetUserId();
        var result = await sender.Send(new GetKitchenQuery(userId));
        if (result == null)
        {
            return BadRequest("Кухня не найдена");
        }

        return Ok(result);
    }

    [AuthorizeMinRank(AppRoles.BranchManager)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetKitchenStaff(string id)
    {
        var result = await sender.Send(new GetKitchenStaffQuery(id));
        if (result == null)
        {
            return BadRequest("Работник кухни не найден");
        }

        return Ok(result);
    }
    
    [AuthorizeMinRank(AppRoles.BranchManager)]
    [HttpGet("{kitchenId:int}")]
    public async Task<IActionResult> GetKitchenStaffs(int kitchenId)
    {
        var result = await sender.Send(new GetKitchenStaffsQuery(kitchenId));
        return Ok(result);
    }
}