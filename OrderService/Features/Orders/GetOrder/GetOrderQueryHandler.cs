using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderDispatch.Contracts.Dtos;
using OrderDispatch.Contracts.Dtos.Orders;
using OrderService.Data;

namespace OrderService.Features.Orders.GetOrder;

public class GetOrderQueryHandler(AppDbContext db) : IRequestHandler<GetOrderQuery, OrderDto?>
{
    public async Task<OrderDto?> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var order = await db.Orders.FirstOrDefaultAsync(
            order => order.UserId == request.UserId && order.Id == request.Id, cancellationToken);

        if (order == null) return null;

        var orderDto = new OrderDto(order.Id, order.UserId, order.Status, order.TotalPrice, order.CreatedAt,
            order.UpdatedAt);

        return orderDto;
    }
}