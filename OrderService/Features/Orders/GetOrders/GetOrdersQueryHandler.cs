using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderDispatch.Contracts.Dtos;
using OrderService.Data;

namespace OrderService.Features.Orders.GetOrders;

public class GetOrdersQueryHandler(AppDbContext db) : IRequestHandler<GetOrdersQuery, List<OrderDto>>
{
    public async Task<List<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await db.Orders.Where(order => order.UserId == request.UserId)
            .ToListAsync(cancellationToken);
        var orderDtos = orders
            .Select(order => new OrderDto(
                order.Id,
                order.UserId,
                order.Status,
                order.TotalPrice,
                order.CreatedAt,
                order.UpdatedAt))
            .ToList();

        return orderDtos;
    }
}