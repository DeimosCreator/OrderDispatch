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

        var allowedKitchen = await db.Kitchens
            .Include(kitchen => kitchen.KitchenOrders)
            .Where(kitchen => kitchen.IsActive &&
                              kitchen.KitchenOrders.Count(ko =>
                                  ko.Status == KitchenStatus.Queued ||
                                  ko.Status == KitchenStatus.Accepted ||
                                  ko.Status == KitchenStatus.Preparing)
                              / (float)kitchen.Capacity < 1f)
            .FirstOrDefaultAsync();

        var kitchenOrder = new Models.Entities.KitchenOrder
        {
            OrderId = msg.OrderId,
            UserId = msg.UserId,
            TotalPrice = msg.TotalPrice,
            GlobalOrderStatus = allowedKitchen != null ? msg.Status : OrderStatus.Rejected,
            KitchenId = allowedKitchen?.Id ?? -1
        };

        if (allowedKitchen != null)
        {
            var activeCount = allowedKitchen.KitchenOrders
                .Count(ko => ko.Status is KitchenStatus.Queued or KitchenStatus.Accepted or KitchenStatus.Preparing);

            allowedKitchen.CurrentLoad = activeCount / (float)allowedKitchen.Capacity;
        }

        db.KitchenOrders.Add(kitchenOrder);
        await db.SaveChangesAsync();
    }
}