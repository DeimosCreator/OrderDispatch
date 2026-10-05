using KitchenService.Data;
using MassTransit;
using OrderDispatch.Contracts.Messaging.Events;

namespace KitchenService.Features.Kitchen.Consumers;

public class OrderCreatedConsumer(AppDbContext db) : IConsumer<OrderCreatedEvent>
{
    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var msg = context.Message;

        var kitchenOrder = new Models.Enities.KitchenOrder
        {
            OrderId = msg.OrderId,
            // Заполняем кэш-данные
            CustomerId = msg.CustomerId,
            TotalPrice = msg.TotalPrice,
            GlobalOrderStatus = msg.Status,
            // TODO: Сделать определение свободной кухни
            KitchenId = 1 
        };

        db.KitchenOrders.Add(kitchenOrder);
        await db.SaveChangesAsync();
    }
}