using KitchenService.Data;
using KitchenService.Models.Enities;
using MassTransit;
using OrderDispatch.Contracts.Messaging.Events;

namespace KitchenService.Features.Kitchen.Consumers;

public class OrderCreatedConsumer(AppDbContext db) : IConsumer<OrderCreatedEvent>
{
    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var msg = context.Message;

        var readOrder = new OrderReadModel
        {
            Id = msg.OrderId,
            CustomerId = msg.CustomerId,
            Status = msg.Status,
            TotalPrice = msg.TotalPrice
        };

        db.OrderReadModels.Add(readOrder);
        await db.SaveChangesAsync();
    }
}