using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Models.Enities.Enums;

namespace OrderService.Features.Orders.GetOrderStatus;

public class GetOrderStatusQueryHandler(AppDbContext db) : IRequestHandler<GetOrderStatusQuery, OrderStatus?>
{
    public async Task<OrderStatus?> Handle(GetOrderStatusQuery request, CancellationToken cancellationToken)
    {
        var order = await db.Orders.FirstOrDefaultAsync(order => order.Id == request.Id, cancellationToken: cancellationToken);

        var status = order?.Status;
        return status;
    }
}