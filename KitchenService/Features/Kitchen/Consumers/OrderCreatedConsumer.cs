using KitchenService.Data;
using KitchenService.Models.Entities;
using KitchenService.Models.Entities.Enums;
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

        var itemsCount = msg.Items.Sum(i => (int)i.Quantity);

        var allowedKitchen = await db.Kitchens
            .Where(k => k.IsActive && k.Capacity > 0 && k.CurrentLoad + (float)itemsCount / k.Capacity <= 1f)
            .OrderBy(k => k.CurrentLoad)
            .FirstOrDefaultAsync(ct);

        var kitchenOrder = new Models.Entities.KitchenOrder
        {
            OrderId = msg.OrderId,
            UserId = msg.UserId,
            TotalPrice = msg.TotalPrice,
            KitchenId = allowedKitchen?.Id,
            KitchenOrderItems = msg.Items.Select(item => new KitchenOrderItem
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            }).ToList()
        };

        if (allowedKitchen != null)
        {
            kitchenOrder.StartedAt = DateTime.UtcNow;
            kitchenOrder.UpdatedAt = DateTime.UtcNow;

            var activeItemsCount = await db.KitchenOrders
                .Where(ko => ko.KitchenId == allowedKitchen.Id &&
                             (ko.Status == KitchenStatus.Accepted || ko.Status == KitchenStatus.Preparing))
                .SelectMany(ko => ko.KitchenOrderItems)
                .SumAsync(i => (int?)i.Quantity, ct) ?? 0;

            allowedKitchen.CurrentLoad = Math.Clamp(
                (float)(activeItemsCount + itemsCount) / allowedKitchen.Capacity,
                0f,
                1f);
        }
        //TODO: Переделать систему под управление взятия заказа шефом

        db.KitchenOrders.Add(kitchenOrder);
        await db.SaveChangesAsync(ct);
    }
}