using KitchenService.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderDispatch.Contracts.Messaging.Events;

namespace KitchenService.Features.Kitchen.Consumers;

public class OrderStatusChangedConsumer(AppDbContext db) : IConsumer<OrderStatusChangedEvent>
{
    public async Task Consume(ConsumeContext<OrderStatusChangedEvent> context)
    {
        var msg = context.Message;

        var kitchenOrder = await db.KitchenOrders
            .FirstOrDefaultAsync(ko => ko.OrderId == msg.OrderId);

        if (kitchenOrder == null)
        {
            throw new InvalidOperationException($"KitchenOrder for Order {msg.OrderId} not found yet.");
        }

        kitchenOrder.GlobalOrderStatus = msg.NewStatus;
        
        await db.SaveChangesAsync();
    }
}