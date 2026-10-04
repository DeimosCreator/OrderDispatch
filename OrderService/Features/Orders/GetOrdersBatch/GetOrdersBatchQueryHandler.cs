using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderDispatch.Contracts.Dtos;
using OrderService.Data;

namespace OrderService.Features.Orders.GetOrdersBatch;

public class GetOrdersBatchQueryHandler(AppDbContext db) : IRequestHandler<GetOrdersBatchQuery, List<OrderDto>>
{
    public async Task<List<OrderDto>> Handle(GetOrdersBatchQuery request, CancellationToken cancellationToken)
    {
        var orders = await db.Orders
            .Include(order => order.OrderItems)
            .Where(order => request.OrderIds.Contains(order.Id))
            .ToListAsync(cancellationToken);

        var orderDtos = orders.Select(order =>
                new OrderDto(order.Id, order.CustomerId, order.Status, order.TotalPrice, order.CreatedAt,
                    order.UpdatedAt))
            .ToList();

        return orderDtos;
    }
}