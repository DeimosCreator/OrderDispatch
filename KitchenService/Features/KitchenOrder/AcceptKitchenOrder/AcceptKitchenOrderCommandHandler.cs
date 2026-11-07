using KitchenService.Data;
using KitchenService.Models.Entities.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KitchenService.Features.KitchenOrder.AcceptKitchenOrder;

public class AcceptKitchenOrderCommandHandler(AppDbContext db) : IRequestHandler<AcceptKitchenOrderCommand, AcceptKitchenOrderResult>
{
    public async Task<AcceptKitchenOrderResult> Handle(AcceptKitchenOrderCommand request, CancellationToken cancellationToken)
    {
        var kitchenOrder = await db.KitchenOrders.Where(kitchenOrder => kitchenOrder.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (kitchenOrder == null) return AcceptKitchenOrderResult.KitchenOrderNotFound;

        var kitchen = await db.Kitchens.Where(kitchen => kitchen.Id == kitchenOrder.KitchenId)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (kitchen == null) return AcceptKitchenOrderResult.KitchenNotFound;

        var kitchenCurrentLoad = kitchen.CurrentLoad + (float)1 / kitchen.Capacity;

        if (kitchenCurrentLoad > 1) return AcceptKitchenOrderResult.KitchenBusy;
        
        if (!kitchenOrder.TransitionTo(KitchenStatus.Accepted)) return AcceptKitchenOrderResult.CannotAccept;

        kitchen.CurrentLoad = kitchenCurrentLoad;
        kitchenOrder.StartedAt = DateTime.UtcNow;
        kitchenOrder.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync(cancellationToken);
        return AcceptKitchenOrderResult.Accepted;
    }
}