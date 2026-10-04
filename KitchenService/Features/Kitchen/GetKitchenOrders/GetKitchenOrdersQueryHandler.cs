using KitchenService.Data;
using KitchenService.Features.Kitchen.Clients.OrderService;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderDispatch.Contracts.Dtos;

namespace KitchenService.Features.Kitchen.GetKitchenOrders;

public class GetKitchenOrdersQueryHandler(AppDbContext db, IOrderHttpClient orderHttpClient)
    : IRequestHandler<GetKitchenOrdersQuery, List<OrderDto>>
{
    public async Task<List<OrderDto>> Handle(GetKitchenOrdersQuery request, CancellationToken cancellationToken)
    {
        var kitchenOrders = await db.KitchenOrders
            .Where(ko => ko.KitchenId == request.Id)
            .ToListAsync(cancellationToken);

        var orderIds = kitchenOrders.Select(ko => ko.OrderId).Distinct().ToList();
        var orders = await orderHttpClient.GetOrders(orderIds, cancellationToken);
        
        var ordersDtos = orders.Select(order =>
                new OrderDto(order.Id, order.CustomerId, order.Status, order.TotalPrice, order.CreatedAt,
                    order.UpdatedAt))
            .ToList();
        
        return ordersDtos;
    }
}