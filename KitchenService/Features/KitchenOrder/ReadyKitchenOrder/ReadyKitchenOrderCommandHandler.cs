using KitchenService.Data;
using KitchenService.Models.Entities.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KitchenService.Features.KitchenOrder.ReadyKitchenOrder;

public class ReadyKitchenOrderCommandHandler(AppDbContext db) 
    : IRequestHandler<ReadyKitchenOrderCommand, ReadyKitchenOrderResult>
{
    public async Task<ReadyKitchenOrderResult> Handle(ReadyKitchenOrderCommand request, CancellationToken cancellationToken)
    {
        var kitchenOrder = await db.KitchenOrders
            .FirstOrDefaultAsync(ko => ko.Id == request.Id, cancellationToken);

        if (kitchenOrder == null) return ReadyKitchenOrderResult.NotFound;

        if (!kitchenOrder.TransitionTo(KitchenStatus.Ready)) return ReadyKitchenOrderResult.CannotReady;

        kitchenOrder.ReadyAt = DateTime.UtcNow;
        kitchenOrder.UpdatedAt = DateTime.UtcNow;

        if (kitchenOrder.KitchenId.HasValue)
        {
            var kitchen = await db.Kitchens
                .FirstOrDefaultAsync(k => k.Id == kitchenOrder.KitchenId.Value, cancellationToken);

            if (kitchen is { Capacity: > 0 })
            {
                var activeCount = await db.KitchenOrders.CountAsync(
                    ko => ko.KitchenId == kitchen.Id &&
                          ko.Id != kitchenOrder.Id &&
                          (ko.Status == KitchenStatus.Accepted || ko.Status == KitchenStatus.Preparing),
                    cancellationToken);

                var queuedOrder = await db.KitchenOrders
                    .Where(o => o.Status == KitchenStatus.Queued && o.KitchenId == null)
                    .OrderBy(o => o.CreatedAt)
                    .FirstOrDefaultAsync(cancellationToken);

                if (queuedOrder != null && activeCount < kitchen.Capacity)
                {
                    queuedOrder.KitchenId = kitchen.Id;
                    queuedOrder.TransitionTo(KitchenStatus.Accepted);
                    queuedOrder.StartedAt = DateTime.UtcNow;
                    activeCount++;
                }

                kitchen.CurrentLoad = Math.Clamp((float)activeCount / kitchen.Capacity, 0f, 1f);
            }
        }

        await db.SaveChangesAsync(cancellationToken);
        return ReadyKitchenOrderResult.Ready;
    }
}