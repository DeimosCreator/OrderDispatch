using KitchenService.Data;
using KitchenService.Models.Entities.Enums;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderDispatch.Contracts.Enums;
using OrderDispatch.Contracts.Messaging.Events.Orders;

namespace KitchenService.Features.Kitchen.Consumers;

public class OrderCreatedConsumer(AppDbContext db) : IConsumer<OrderCreatedEvent>
{
    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var msg = context.Message;
        var ct = context.CancellationToken;

        if (await db.KitchenOrders.AnyAsync(ko => ko.OrderId == msg.OrderId, ct))
            return;

        var allowedKitchen = await db.Kitchens
            .Where(k => k.IsActive && k.Capacity > 0 && k.CurrentLoad < 1f)
            .OrderBy(k => k.CurrentLoad)
            .FirstOrDefaultAsync(ct);

        var kitchenOrder = new Models.Entities.KitchenOrder
        {
            OrderId = msg.OrderId,
            UserId = msg.UserId,
            TotalPrice = msg.TotalPrice,
            GlobalOrderStatus = msg.Status,
            KitchenId = allowedKitchen?.Id
        };

        if (allowedKitchen != null)
        {
            kitchenOrder.TransitionTo(KitchenStatus.Accepted);
            kitchenOrder.StartedAt = DateTime.UtcNow;

            var activeCount = await db.KitchenOrders.CountAsync(
                ko => ko.KitchenId == allowedKitchen.Id &&
                      (ko.Status == KitchenStatus.Accepted || ko.Status == KitchenStatus.Preparing),
                ct);

            allowedKitchen.CurrentLoad = Math.Clamp((float)(activeCount + 1) / allowedKitchen.Capacity, 0f, 1f);
        }

        db.KitchenOrders.Add(kitchenOrder);
        await db.SaveChangesAsync(ct);
    }
}