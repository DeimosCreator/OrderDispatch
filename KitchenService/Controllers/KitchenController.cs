using KitchenService.Features.Kitchen.CreateKitchen;
using KitchenService.Features.Kitchen.GetKitchen;
using KitchenService.Features.Kitchen.GetKitchenLoad;
using KitchenService.Features.Kitchen.GetKitchenOrders;
using KitchenService.Features.Kitchen.GetKitchens;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderDispatch.Contracts.Constants;
using OrderDispatch.Contracts.Security;

namespace KitchenService.Controllers;

[ApiController]
[Route("api/kitchens")]
public class KitchenController(ISender sender) : ControllerBase
{
    [AuthorizeMinRank(AppRoles.CompanyOwner)]
    [HttpPost]
    public async Task<IActionResult> CreateKitchen([FromBody] CreateKitchenCommand createKitchenCommand)
    {
        var kitchen = await sender.Send(createKitchenCommand);

        return CreatedAtAction(
            nameof(GetKitchen),
            new { id = kitchen.Id },
            kitchen);
    }

    [AuthorizeMinRank(AppRoles.BranchManager)]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetKitchen(int id)
    {
        var kitchen = await sender.Send(new GetKitchenQuery(id));
        if (kitchen == null)
        {
            return BadRequest("Кухня не найдена");
        }

        return Ok(kitchen);
    }

    [AuthorizeMinRank(AppRoles.CompanyOwner)]
    [HttpGet]
    public async Task<IActionResult> GetKitchens()
    {
        var kitchens = await sender.Send(new GetKitchensQuery());
        return Ok(kitchens);
    }

    [AuthorizeMinRank(AppRoles.BranchManager)]
    [HttpGet("{id:int}/load")]
    public async Task<IActionResult> GetKitchenLoad(int id)
    {
        var load = await sender.Send(new GetKitchenLoadQuery(id));
        if (load == null)
        {
            return BadRequest("Кухня не найдена");
        }

        return Ok(load);
    }

    [AuthorizeMinRank(AppRoles.Chef)]
    [HttpGet("{id:int}/orders")]
    public async Task<IActionResult> GetKitchenOrders(int id)
    {
        var orders = await sender.Send(new GetKitchenOrdersQuery(id));
        return Ok(orders);
    }
}