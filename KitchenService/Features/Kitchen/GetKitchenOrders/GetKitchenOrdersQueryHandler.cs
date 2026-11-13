using KitchenService.Data;
using KitchenService.Models.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KitchenService.Features.Kitchen.GetKitchenOrders;

public class GetKitchenOrdersQueryHandler(AppDbContext db)
    : IRequestHandler<GetKitchenOrdersQuery, List<KitchenOrderDto>>
{
    public async Task<List<KitchenOrderDto>> Handle(GetKitchenOrdersQuery request, CancellationToken cancellationToken)
    {
        return await db.KitchenOrders
            .AsNoTracking()
            .Where(ko => ko.KitchenId == request.Id)
            .Select(ko => new KitchenOrderDto(
                ko.Id,
                ko.OrderId,
                ko.KitchenId,
                ko.Status,
                ko.CreatedAt,
                ko.UpdatedAt,
                ko.StartedAt,
                ko.ReadyAt
            ))
            .ToListAsync(cancellationToken);
    }
}