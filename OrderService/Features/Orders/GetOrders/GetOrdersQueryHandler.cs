using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Models.Dtos;

namespace OrderService.Features.Orders.GetOrders;

public class GetOrdersQueryHandler(AppDbContext db) : IRequestHandler<GetOrdersQuery, List<OrderDto>>
{
    public async Task<List<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await db.Orders.ToListAsync(cancellationToken: cancellationToken);
        var orderDtos = orders
            .Select(order => new OrderDto(
                order.Id,
                order.CustomerId,
                order.Status,
                order.TotalPrice,
                order.CreatedAt,
                order.UpdatedAt))
            .ToList();

        return orderDtos;
    }
}