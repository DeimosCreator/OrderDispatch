using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Models.Dtos;
using OrderService.Models.Enities;
using OrderService.Models.Enities.Enums;
using OrderService.Services.Interfaces;

namespace OrderService.Services;

public class OrderService : IOrderService
{
    private readonly AppDbContext _db;

    public OrderService(AppDbContext db)
    {
        _db = db;
    }
    
    public async Task<List<OrderDto>> GetOrders()
    {
        var orders = await _db.Orders.ToListAsync();
        var orderDtos = orders
            .Select(order => new OrderDto(
                order.Id,
                order.CustomerId,
                order.Status,
                order.TotalPrice,
                order.CreatedAt,
                order.UpdatedAt)).ToList();

        return orderDtos;
    }
    
    public async Task<OrderDto> CreateOrder(CreateOrderDto createOrderDto)
    {
        var order = new Order
        {
            CustomerId = createOrderDto.CustomerId,
            TotalPrice = createOrderDto.TotalPrice
        };

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        var orderDto = new OrderDto(order.Id, order.CustomerId, order.Status, order.TotalPrice, order.CreatedAt,
            order.UpdatedAt);
        
        return orderDto;
    }

    public async Task<OrderDto?> GetOrder(int id)
    {
        var order = await _db.Orders.FirstOrDefaultAsync(order => order.Id == id);

        if (order == null)
        {
            return null;
        }
        
        var orderDto = new OrderDto(order.Id, order.CustomerId, order.Status, order.TotalPrice, order.CreatedAt,
            order.UpdatedAt);

        return orderDto;
    }

    public async Task<OrderStatus?> GetOrderStatus(int id)
    {
        var order = await _db.Orders.FirstOrDefaultAsync(order => order.Id == id);

        var status = order?.Status;
        return status;
    }

    public async Task<PayOrderResult> PayOrder(int id)
    {
        var order = await _db.Orders.FirstOrDefaultAsync(order => order.Id == id);

        if (order == null)
        {
            return PayOrderResult.NotFound;
        }
        
        if (!order.TransitionTo(OrderStatus.Paid)) return PayOrderResult.CannotPaid;
        
        await _db.SaveChangesAsync();
        return PayOrderResult.Paid;
    }

    public async Task<CancelOrderResult> CancelOrder(int id)
    {
        var order = await _db.Orders.FirstOrDefaultAsync(order => order.Id == id);

        if (order == null)
        {
            return CancelOrderResult.NotFound;
        }

        if (!order.TransitionTo(OrderStatus.Cancelled)) return CancelOrderResult.CannotCancel;
        
        await _db.SaveChangesAsync();
        return CancelOrderResult.Cancelled;
    }
}