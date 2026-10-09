using KitchenService.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderDispatch.Contracts.Dtos;

namespace KitchenService.Features.Kitchen.GetKitchenOrders;

public class GetKitchenOrdersQueryHandler(AppDbContext db)
    : IRequestHandler<GetKitchenOrdersQuery, List<OrderDto>>
{
    public async Task<List<OrderDto>> Handle(GetKitchenOrdersQuery request, CancellationToken cancellationToken)
    {
        var ordersDtos = await db.KitchenOrders
            .Where(ko => ko.KitchenId == request.Id)
            .Select(ko => new OrderDto(
                ko.OrderId,
                ko.UserId,
                ko.GlobalOrderStatus,
                ko.TotalPrice,
                ko.CreatedAt,
                ko.UpdatedAt
            ))
            .ToListAsync(cancellationToken);

        return ordersDtos;
    }
}