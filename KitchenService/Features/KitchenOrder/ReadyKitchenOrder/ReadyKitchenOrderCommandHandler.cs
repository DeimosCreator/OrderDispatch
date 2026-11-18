using KitchenService.Data;
using KitchenService.Models.Entities.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KitchenService.Features.KitchenOrder.ReadyKitchenOrder;

public class ReadyKitchenOrderCommandHandler(AppDbContext db)
    : IRequestHandler<ReadyKitchenOrderCommand, ReadyKitchenOrderResult>
{
    public async Task<ReadyKitchenOrderResult> Handle(
        ReadyKitchenOrderCommand request,
        CancellationToken cancellationToken)
    {
        var kitchenOrder = await db.KitchenOrders
            .FirstOrDefaultAsync(ko => ko.Id == request.Id, cancellationToken);

        if (kitchenOrder == null)
            return ReadyKitchenOrderResult.KitchenOrderNotFound;

        if (!kitchenOrder.TransitionTo(KitchenStatus.Ready))
            return ReadyKitchenOrderResult.CannotReady;

        kitchenOrder.ReadyAt = DateTime.UtcNow;

        if (kitchenOrder.KitchenId.HasValue)
        {
            var kitchen = await db.Kitchens
                .FirstOrDefaultAsync(k => k.Id == kitchenOrder.KitchenId.Value, cancellationToken);

            if (kitchen == null)
                return ReadyKitchenOrderResult.KitchenNotFound;

            if (kitchen.Capacity > 0)
            {
                var activeItemsCount = await db.KitchenOrders
                    .Where(ko => ko.KitchenId == kitchen.Id &&
                                 ko.Id != kitchenOrder.Id &&
                                 (ko.Status == KitchenStatus.Accepted || ko.Status == KitchenStatus.Preparing))
                    .SelectMany(ko => ko.KitchenOrderItems)
                    .SumAsync(i => (int?)i.Quantity, cancellationToken) ?? 0;

                var queuedOrders = await db.KitchenOrders
                    .Include(o => o.KitchenOrderItems)
                    .Where(o => o.Status == KitchenStatus.Queued && o.KitchenId == null)
                    .OrderBy(o => o.CreatedAt)
                    .ToListAsync(cancellationToken);

                var nextOrder = queuedOrders.FirstOrDefault(o =>
                {
                    var orderItemsCount = o.KitchenOrderItems.Sum(i => (int)i.Quantity);
                    return activeItemsCount + orderItemsCount <= kitchen.Capacity;
                });

                if (nextOrder != null)
                {
                    nextOrder.KitchenId = kitchen.Id;
                    nextOrder.TransitionTo(KitchenStatus.Accepted);
                    nextOrder.StartedAt = DateTime.UtcNow;

                    activeItemsCount += nextOrder.KitchenOrderItems.Sum(i => (int)i.Quantity);
                }

                kitchen.CurrentLoad = Math.Clamp((float)activeItemsCount / kitchen.Capacity, 0f, 1f);
            }
        }

        await db.SaveChangesAsync(cancellationToken);
        return ReadyKitchenOrderResult.Ready;
    }
}