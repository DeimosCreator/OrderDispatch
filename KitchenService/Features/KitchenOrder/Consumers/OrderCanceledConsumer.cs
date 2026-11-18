using KitchenService.Data;
using KitchenService.Models.Entities.Enums;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderDispatch.Contracts.Messaging.Events.Orders;

namespace KitchenService.Features.KitchenOrder.Consumers;

public class OrderCanceledConsumer(AppDbContext db) : IConsumer<OrderChangedEvent>
{
    public async Task Consume(ConsumeContext<OrderChangedEvent> context)
    {
        var msg = context.Message;
        var ct = context.CancellationToken;

        var kitchenOrder = await db.KitchenOrders
            .Where(order => order.OrderId == msg.OrderId)
            .FirstOrDefaultAsync(ct);
        
        if (kitchenOrder == null)
        {
            throw new InvalidOperationException($"KitchenOrder for Order {msg.OrderId} not found yet.");
        }

        kitchenOrder.TransitionTo(KitchenStatus.Cancelled);
        
        await db.SaveChangesAsync(ct);
    }
}