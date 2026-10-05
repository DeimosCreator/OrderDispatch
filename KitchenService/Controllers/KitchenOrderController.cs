using KitchenService.Features.KitchenOrder.AcceptKitchenOrder;
using KitchenService.Features.KitchenOrder.CancelKitchenOrder;
using KitchenService.Features.KitchenOrder.GetKitchenOrder;
using KitchenService.Features.KitchenOrder.PrepareKitchenOrder;
using KitchenService.Features.KitchenOrder.ReadyKitchenOrder;
using KitchenService.Models.Enities.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KitchenService.Controllers;

[ApiController]
[Route("api/kitchen-orders")]
public class KitchenOrderController(ISender sender) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetKitchenOrder(int id)
    {
        var kitchenOrder = await sender.Send(new GetKitchenOrderQuery(id));
        if (kitchenOrder == null)
        {
            return BadRequest("Заказ кухни не найден");
        }

        return Ok(kitchenOrder);
    }

    [HttpPost("{id:int}/accept")]
    public async Task<IActionResult> AcceptKitchenOrder(int id)
    {
        var status = await sender.Send(new AcceptKitchenOrderCommand(id));
        return status switch
        {
            AcceptKitchenOrderResult.NotFound => BadRequest("Заказ кухни не найден"),
            AcceptKitchenOrderResult.CannotAccept => Conflict("Заказ кухни нельзя взять"),
            AcceptKitchenOrderResult.Accepted => Ok(status),
            _ => StatusCode(500)
        };
    }
    
    [HttpPost("{id:int}/prepare")]
    public async Task<IActionResult> PrepareKitchenOrder(int id)
    {
        var status = await sender.Send(new PrepareKitchenOrderCommand(id));
        return status switch
        {
            PrepareKitchenOrderResult.NotFound => BadRequest("Заказ кухни не найден"),
            PrepareKitchenOrderResult.CannotPrepare => Conflict("Заказ кухни нельзя приготовить"),
            PrepareKitchenOrderResult.Preparing => Ok(status),
            _ => StatusCode(500)
        };
    }
    
    [HttpPost("{id:int}/ready")]
    public async Task<IActionResult> ReadyKitchenOrder(int id)
    {
        var status = await sender.Send(new ReadyKitchenOrderCommand(id));
        return status switch
        {
            ReadyKitchenOrderResult.NotFound => BadRequest("Заказ кухни не найден"),
            ReadyKitchenOrderResult.CannotReady => Conflict("Заказ кухни нельзя назначить готовым"),
            ReadyKitchenOrderResult.Ready => Ok(status),
            _ => StatusCode(500)
        };
    }
    
    [HttpPost("{id:int}/cancel")]
    public async Task<IActionResult> CancelKitchenOrder(int id)
    {
        var status = await sender.Send(new CancelKitchenOrderCommand(id));
        return status switch
        {
            CancelKitchenOrderResult.NotFound => BadRequest("Заказ кухни не найден"),
            CancelKitchenOrderResult.CannotCancel => Conflict("Заказ кухни нельзя отменить"),
            CancelKitchenOrderResult.Cancelled => Ok(status),
            _ => StatusCode(500)
        };
    }
}