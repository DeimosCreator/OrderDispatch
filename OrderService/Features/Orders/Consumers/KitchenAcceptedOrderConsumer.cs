using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderDispatch.Contracts.Enums;
using OrderDispatch.Contracts.Messaging.Events.Kitchens;
using OrderService.Data;

namespace OrderService.Features.Orders.Consumers;

public class KitchenAcceptedOrderConsumer(AppDbContext db) : IConsumer<KitchenAcceptedOrderEvent>
{
    public async Task Consume(ConsumeContext<KitchenAcceptedOrderEvent> context)
    {
        var msg = context.Message;
        var ct = context.CancellationToken;
        
        var order = await db.Orders
            .Where(order => order.Id == msg.OrderId)
            .FirstOrDefaultAsync(ct);

        if (order == null)
        {
            throw new InvalidOperationException($"Order {msg.OrderId} not found yet.");
        }

        order.TransitionTo(OrderStatus.KitchenAccepted);
        await db.SaveChangesAsync(ct);
    }
}