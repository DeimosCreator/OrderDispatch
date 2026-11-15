using KitchenService.Data;
using KitchenService.Models.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderDispatch.Contracts.Messaging.Events.Orders;

namespace KitchenService.Features.Kitchen.Consumers;

public class OrderCreatedConsumer(AppDbContext db) : IConsumer<OrderCreatedEvent>
{
    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var msg = context.Message;
        var ct = context.CancellationToken;
        
        if (await db.KitchenOrders.AnyAsync(ko => ko.OrderId == msg.OrderId, ct))
            return;

        var kitchenOrder = new Models.Entities.KitchenOrder
        {
            OrderId = msg.OrderId,
            UserId = msg.UserId,
            TotalPrice = msg.TotalPrice,
            KitchenId = null,
            KitchenOrderItems = msg.Items.Select(item => new KitchenOrderItem
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            }).ToList()
        };

        db.KitchenOrders.Add(kitchenOrder);
        await db.SaveChangesAsync(ct);
    }
}