using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Features.Orders.CancelOrder;
using OrderService.Features.Orders.CreateOrder;
using OrderService.Features.Orders.GetOrder;
using OrderService.Features.Orders.GetOrders;
using OrderService.Features.Orders.GetOrdersBatch;
using OrderService.Features.Orders.GetOrderStatus;
using OrderService.Features.Orders.PayOrder;
using OrderService.Models.Enities.Enums;

namespace OrderService.Controllers;

[ApiController]
[Route("api/orders")]
[Authorize]
public class OrderController(ISender sender) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await sender.Send(new GetOrdersQuery());
        return Ok(orders);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand createOrderCommand)
    {
        var userId = GetUserId();
        var order = await sender.Send(createOrderCommand with { UserId = userId });
        return CreatedAtAction(
            nameof(GetOrder),
            new {id = order.Id},
            order);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var order = await sender.Send(new GetOrderQuery(id));
        if (order == null)
        {
            return BadRequest("Заказ не найден");
        }
        
        return Ok(order);
    }

    [HttpGet("batch")]
    public async Task<IActionResult> GetOrdersBatch([FromBody] GetOrdersBatchQuery ordersBatchQuery)
    {
        var orders = await sender.Send(ordersBatchQuery);
        return Ok(orders);
    }
    
    [HttpGet("{id:int}/status")]
    public async Task<IActionResult> GetOrderStatus(int id)
    {
        var status = await sender.Send(new GetOrderStatusQuery(id));
        if (status == null)
        {
            return BadRequest("Заказ не найден");
        }
        
        return Ok(status);
    }
    
    [HttpPost("{id:int}/pay")]
    public async Task<IActionResult> PayOrder(int id)
    {
        var result = await sender.Send(new PayOrderCommand(id));

        return result switch
        {
            PayOrderResult.NotFound =>
                NotFound("Заказ не найден"),

            PayOrderResult.CannotPaid =>
                Conflict("Заказ нельзя оплатить"),

            PayOrderResult.Paid =>
                Ok("Заказ оплачен"),

            _ => StatusCode(500)
        };
    }
    
    [HttpPost("{id:int}/cancel")]
    public async Task<IActionResult> CancelOrder(int id)
    {
        var result = await sender.Send(new CancelOrderCommand(id));

        return result switch
        {
            CancelOrderResult.NotFound =>
                NotFound("Заказ не найден"),

            CancelOrderResult.CannotCancel =>
                Conflict("Заказ нельзя отменить"),

            CancelOrderResult.Cancelled =>
                Ok("Заказ отменен"),

            _ => StatusCode(500)
        };
    }
}