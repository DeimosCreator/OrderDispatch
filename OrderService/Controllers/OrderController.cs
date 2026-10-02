using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Features.Orders.CancelOrder;
using OrderService.Features.Orders.CreateOrder;
using OrderService.Features.Orders.GetOrder;
using OrderService.Features.Orders.GetOrders;
using OrderService.Features.Orders.GetOrderStatus;
using OrderService.Features.Orders.PayOrder;
using OrderService.Models.Enities.Enums;

namespace OrderService.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _mediator.Send(new GetOrdersQuery());
        return Ok(orders);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand createOrderCommand)
    {
        var order = await _mediator.Send(createOrderCommand);
        return CreatedAtAction(
            nameof(GetOrder),
            new {id = order.Id},
            order);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var order = await _mediator.Send(new GetOrderQuery(id));
        if (order == null)
        {
            return BadRequest("Заказ не найден");
        }
        
        return Ok(order);
    }
    
    [HttpGet("{id:int}/status")]
    public async Task<IActionResult> GetOrderStatus(int id)
    {
        var status = await _mediator.Send(new GetOrderStatusQuery(id));
        if (status == null)
        {
            return BadRequest("Заказ не найден");
        }
        
        return Ok(status);
    }
    
    [HttpPost("{id:int}/pay")]
    public async Task<IActionResult> PayOrder(int id)
    {
        var result = await _mediator.Send(new PayOrderCommand(id));

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
        var result = await _mediator.Send(new CancelOrderCommand(id));

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