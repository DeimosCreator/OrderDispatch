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

        var orderRead = await db.OrderReadModels
            .Where(orderRead => orderRead.Id == msg.OrderId)
            .FirstOrDefaultAsync();

        if (orderRead == null)
        {
            throw new InvalidOperationException($"Order {msg.OrderId} not found yet.");
        }

        orderRead.Status = msg.NewStatus;
        await db.SaveChangesAsync();
    }
}