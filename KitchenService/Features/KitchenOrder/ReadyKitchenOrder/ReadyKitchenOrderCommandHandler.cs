using KitchenService.Data;
using KitchenService.Models.Enities.Enums;
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

        await db.SaveChangesAsync(cancellationToken);
        return ReadyKitchenOrderResult.Ready;
    }
}