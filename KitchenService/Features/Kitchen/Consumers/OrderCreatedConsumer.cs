using KitchenService.Data;
using KitchenService.Models.Enities.Enums;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderDispatch.Contracts.Enums;
using OrderDispatch.Contracts.Messaging.Events;

namespace KitchenService.Features.Kitchen.Consumers;

public class OrderCreatedConsumer(AppDbContext db) : IConsumer<OrderCreatedEvent>
{
    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var msg = context.Message;
        
        var allowedKitchen = await db.Kitchens
            .Include(kitchen => kitchen.KitchenOrders)
            .Where(kitchen => kitchen.IsActive && 
                              (kitchen.KitchenOrders.Count(ko => 
                                   ko.Status == KitchenStatus.Accepted || 
                                   ko.Status == KitchenStatus.Preparing) + 1) 
                              / (float)kitchen.Capacity <= 1f)
            .FirstOrDefaultAsync();

        var kitchenOrder = new Models.Enities.KitchenOrder
        {
            OrderId = msg.OrderId,
            CustomerId = msg.CustomerId,
            TotalPrice = msg.TotalPrice,
            GlobalOrderStatus = allowedKitchen != null ? msg.Status : OrderStatus.Rejected, 
            KitchenId = allowedKitchen?.Id ?? -1
        };

        if (allowedKitchen != null)
        {
            var activeCount = allowedKitchen.KitchenOrders
                .Count(ko => ko.Status is KitchenStatus.Accepted or KitchenStatus.Preparing) + 1;
                
            allowedKitchen.CurrentLoad = activeCount / (float)allowedKitchen.Capacity;
        }

        db.KitchenOrders.Add(kitchenOrder);
        await db.SaveChangesAsync();
    }
}