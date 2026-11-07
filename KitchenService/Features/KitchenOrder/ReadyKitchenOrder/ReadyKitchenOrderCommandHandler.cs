using KitchenService.Data;
using KitchenService.Models.Entities.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KitchenService.Features.KitchenOrder.ReadyKitchenOrder;

public class ReadyKitchenOrderCommandHandler(AppDbContext db) 
    : IRequestHandler<ReadyKitchenOrderCommand, ReadyKitchenOrderResult>
{
    public async Task<ReadyKitchenOrderResult> Handle(ReadyKitchenOrderCommand request, CancellationToken cancellationToken)
    {
        var kitchenOrder = await db.KitchenOrders.Where(kitchenOrder => kitchenOrder.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (kitchenOrder == null) return ReadyKitchenOrderResult.NotFound;
        
        if (!kitchenOrder.TransitionTo(KitchenStatus.Ready)) return ReadyKitchenOrderResult.CannotReady;

        var queuedKitchenOrder = await db.KitchenOrders
            .Where(order => order.Status == KitchenStatus.Queued && order.KitchenId == null)
            .OrderByDescending(order => order.UpdatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        if (queuedKitchenOrder != null)
        {
            queuedKitchenOrder.KitchenId = kitchenOrder.KitchenId;
            queuedKitchenOrder.UpdatedAt = DateTime.UtcNow;
        }
        
        kitchenOrder.UpdatedAt = DateTime.UtcNow;
        
        await db.SaveChangesAsync(cancellationToken);
        return ReadyKitchenOrderResult.Ready;
    }
}