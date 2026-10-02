using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Models.Dtos;

namespace OrderService.Features.Orders.GetOrder;

public class GetOrderQueryHandler(AppDbContext db) : IRequestHandler<GetOrderQuery, OrderDto?>
{
    public async Task<OrderDto?> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var order = await db.Orders.FirstOrDefaultAsync(order => order.Id == request.Id, cancellationToken: cancellationToken);

        if (order == null) return null;
        
        var orderDto = new OrderDto(order.Id, order.CustomerId, order.Status, order.TotalPrice, order.CreatedAt,
            order.UpdatedAt);

        return orderDto;
    }
}