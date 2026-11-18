using KitchenService.Data;
using KitchenService.Models.Entities.Enums;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderDispatch.Contracts.Messaging.Events.Orders;

namespace KitchenService.Features.KitchenOrder.Consumers;

public class OrderPaidConsumer(AppDbContext db) : IConsumer<OrderPaidEvent>
{
    public async Task Consume(ConsumeContext<OrderPaidEvent> context)
    {
        var msg = context.Message;
        var ct = context.CancellationToken;

        var kitchenOrder = await db.KitchenOrders
            .Where(kitchenOrder => kitchenOrder.OrderId == msg.OrderId)
            .FirstOrDefaultAsync(ct);

        if (kitchenOrder == null)
        {
            throw new InvalidOperationException($"KitchenOrder for Order {msg.OrderId} not found yet.");
        }

        kitchenOrder.TransitionTo(KitchenStatus.Paid);
        
        await db.SaveChangesAsync(ct);
    }
}