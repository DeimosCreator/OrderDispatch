using Microsoft.AspNetCore.Mvc;
using OrderService.Models.Dtos;
using OrderService.Models.Enities.Enums;
using OrderService.Services.Interfaces;

namespace OrderService.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _service;

    public OrderController(IOrderService service)
    {
        _service = service;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _service.GetOrders();
        return Ok(orders);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
    {
        var order = await _service.CreateOrder(orderDto);
        return CreatedAtAction(
            nameof(GetOrder),
            new {id = order.Id},
            order);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var order = await _service.GetOrder(id);
        if (order == null)
        {
            return BadRequest("Заказ не найден");
        }
        
        return Ok(order);
    }
    
    [HttpGet("{id:int}/status")]
    public async Task<IActionResult> GetOrderStatus(int id)
    {
        var status = await _service.GetOrderStatus(id);
        if (status == null)
        {
            return BadRequest("Заказ не найден");
        }
        
        return Ok(status);
    }
    
    [HttpPost("{id:int}/pay")]
    public async Task<IActionResult> PayOrder(int id)
    {
        var result = await _service.PayOrder(id);

        return result switch
        {
            PayOrderResult.NotFound =>
                NotFound("Заказ не найден"),

            PayOrderResult.CannotPaid =>
                Conflict("Заказ нельзя оплатить"),

            PayOrderResult.Paid =>
                Ok("Заказ отменен"),

            _ => StatusCode(500)
        };
    }
    
    [HttpPost("{id:int}/cancel")]
    public async Task<IActionResult> CancelOrder(int id)
    {
        var result = await _service.CancelOrder(id);

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