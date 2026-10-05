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
        var query = db.KitchenOrders
            .Where(ko => ko.KitchenId == request.Id)
            .Join(
                db.OrderReadModels,
                ko => ko.OrderId,
                o => o.Id,
                (ko, o) => new OrderDto(
                    o.Id, 
                    o.CustomerId, 
                    o.Status, 
                    o.TotalPrice, 
                    ko.CreatedAt, 
                    ko.UpdatedAt
                )
            );

        var ordersDtos = await query.ToListAsync(cancellationToken);
        
        return ordersDtos;
    }
}