using KitchenService.Data;
using KitchenService.Models.Entities.Enums;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderDispatch.Contracts.Enums;
using OrderDispatch.Contracts.Messaging.Events.Orders;

namespace KitchenService.Features.Kitchen.Consumers;

public class OrderStatusChangedConsumer(AppDbContext db) : IConsumer<OrderStatusChangedEvent>
{
    public async Task Consume(ConsumeContext<OrderStatusChangedEvent> context)
    {
        var msg = context.Message;
        var ct = context.CancellationToken;

        var kitchenOrder = await db.KitchenOrders
            .FirstOrDefaultAsync(ko => ko.OrderId == msg.OrderId, ct);

        if (kitchenOrder == null)
        {
            throw new InvalidOperationException($"KitchenOrder for Order {msg.OrderId} not found yet.");
        }

        if (kitchenOrder.TransitionTo(msg.NewStatus)) kitchenOrder.UpdatedAt = msg.UpdatedAt;

        if (msg.NewStatus == OrderStatus.Cancelled)
        {
            var wasActive = kitchenOrder.Status is KitchenStatus.Accepted or KitchenStatus.Preparing;
            if (kitchenOrder.TransitionTo(KitchenStatus.Cancelled) && wasActive && kitchenOrder.KitchenId.HasValue)
            {
                var kitchen = await db.Kitchens.FirstOrDefaultAsync(k => k.Id == kitchenOrder.KitchenId.Value, ct);
                if (kitchen is { Capacity: > 0 })
                {
                    var activeItemsCount = await db.KitchenOrders
                        .Where(ko => ko.KitchenId == kitchen.Id &&
                                     ko.Id != kitchenOrder.Id &&
                                     (ko.Status == KitchenStatus.Accepted || ko.Status == KitchenStatus.Preparing))
                        .SelectMany(ko => ko.KitchenOrderItems)
                        .SumAsync(i => (int?)i.Quantity, ct) ?? 0;
                    
                    kitchen.CurrentLoad = Math.Clamp((float)activeItemsCount / kitchen.Capacity, 0f, 1f);
                }
            }
        }

        await db.SaveChangesAsync(ct);
    }
}