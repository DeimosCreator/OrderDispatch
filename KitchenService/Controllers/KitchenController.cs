using KitchenService.Features.Kitchen.CreateKitchen;
using KitchenService.Features.Kitchen.GetKitchen;
using KitchenService.Features.Kitchen.GetKitchens;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KitchenService.Controllers;

[ApiController]
[Route("api/kitchens")]
public class KitchenController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateKitchen([FromBody] CreateKitchenCommand createKitchenCommand)
    {
        var kitchen = await sender.Send(createKitchenCommand);

        return CreatedAtAction(
            nameof(GetKitchen),
            new { id = kitchen.Id },
            kitchen);
    }

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

    [HttpGet]
    public async Task<IActionResult> GetKitchens(GetKitchensQuery getKitchensQuery)
    {
        var kitchens = await sender.Send(getKitchensQuery);
        return Ok(kitchens);
    }
}