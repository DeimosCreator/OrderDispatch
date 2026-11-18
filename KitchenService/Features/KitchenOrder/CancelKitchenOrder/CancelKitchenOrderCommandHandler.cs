using KitchenService.Data;
using KitchenService.Models.Entities.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KitchenService.Features.KitchenOrder.CancelKitchenOrder;

public class CancelKitchenOrderCommandHandler(AppDbContext db) 
    : IRequestHandler<CancelKitchenOrderCommand, CancelKitchenOrderResult>
{
    public async Task<CancelKitchenOrderResult> Handle(CancelKitchenOrderCommand request, CancellationToken cancellationToken)
    {
        var kitchenOrder = await db.KitchenOrders.Where(kitchenOrder => kitchenOrder.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (kitchenOrder == null) return CancelKitchenOrderResult.NotFound;
        
        var wasActive = kitchenOrder.Status is KitchenStatus.Accepted or KitchenStatus.Preparing;
        
        if (!kitchenOrder.TransitionTo(KitchenStatus.Cancelled)) return CancelKitchenOrderResult.CannotCancel;

        if (wasActive && kitchenOrder.KitchenId.HasValue)
        {
            var kitchen = await db.Kitchens
                .FirstOrDefaultAsync(k => k.Id == kitchenOrder.KitchenId.Value, cancellationToken);

            if (kitchen is { Capacity: > 0 })
            {
                var activeItemsCount = await db.KitchenOrders
                    .Where(ko => ko.KitchenId == kitchen.Id &&
                                 ko.Id != kitchenOrder.Id &&
                                 (ko.Status == KitchenStatus.Accepted || ko.Status == KitchenStatus.Preparing))
                    .SelectMany(ko => ko.KitchenOrderItems)
                    .SumAsync(i => (int?)i.Quantity, cancellationToken) ?? 0; 
                
                kitchen.CurrentLoad = Math.Clamp((float)activeItemsCount / kitchen.Capacity, 0f, 1f);
            }
        }

        await db.SaveChangesAsync(cancellationToken);
        return CancelKitchenOrderResult.Cancelled;
    }
}