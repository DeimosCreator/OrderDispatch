using KitchenService.Data;
using KitchenService.Models.Entities.Enums;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderDispatch.Contracts.Messaging.Events.Kitchens;

namespace KitchenService.Features.KitchenOrder.AcceptKitchenOrder;

public class AcceptKitchenOrderCommandHandler(AppDbContext db, IPublishEndpoint publishEndpoint)
    : IRequestHandler<AcceptKitchenOrderCommand, AcceptKitchenOrderResult>
{
    public async Task<AcceptKitchenOrderResult> Handle(AcceptKitchenOrderCommand request, CancellationToken cancellationToken)
    {
        var kitchenOrder = await db.KitchenOrders.Where(kitchenOrder => kitchenOrder.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (kitchenOrder == null) return AcceptKitchenOrderResult.KitchenOrderNotFound;

        var kitchen = await db.Kitchens
            .Where(kitchen => kitchen.KitchenStaffs.Any(kitchenStaff => kitchenStaff.UserId == request.UserId))
            .FirstOrDefaultAsync(cancellationToken);

        if (kitchen == null) return AcceptKitchenOrderResult.KitchenNotFound;
        
        if (!kitchen.IsActive) return AcceptKitchenOrderResult.KitchenNotActive;
        
        if (kitchen.Capacity <= 0) return AcceptKitchenOrderResult.KitchenBusy;
        
        var orderItemsCount = await db.KitchenOrderItems
            .Where(i => i.KitchenOrderId == kitchenOrder.Id)
            .SumAsync(i => (int?)i.Quantity, cancellationToken) ?? 0;

        var activeItemsCount = await db.KitchenOrders
            .Where(ko => ko.KitchenId == kitchen.Id &&
                         (ko.Status == KitchenStatus.Accepted || ko.Status == KitchenStatus.Preparing))
            .SelectMany(ko => ko.KitchenOrderItems)
            .SumAsync(i => (int?)i.Quantity, cancellationToken) ?? 0;

        if (activeItemsCount + orderItemsCount > kitchen.Capacity)
            return AcceptKitchenOrderResult.KitchenBusy;

        if (!kitchenOrder.TransitionTo(KitchenStatus.Accepted))
            return AcceptKitchenOrderResult.CannotAccept;

        kitchen.CurrentLoad = Math.Clamp((float)(activeItemsCount + orderItemsCount) / kitchen.Capacity, 0f, 1f);

        await db.SaveChangesAsync(cancellationToken);

        await publishEndpoint.Publish(new KitchenAcceptedOrderEvent(kitchenOrder.OrderId), cancellationToken);
        
        return AcceptKitchenOrderResult.Accepted;
    }
}