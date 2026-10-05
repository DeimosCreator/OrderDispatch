using KitchenService.Data;
using KitchenService.Models.Enities.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KitchenService.Features.KitchenOrder.CancelKitchenOrder;

public class CancelKitchenOrderCommandHandler(AppDbContext db) 
    : IRequestHandler<CancelKitchenOrderCommand, CancelKitchenOrderResult>
{
    public async Task<CancelKitchenOrderResult> Handle(CancelKitchenOrderCommand request, CancellationToken cancellationToken)
    {
        var kitchenOrder = await db.KitchenOrders.Where(kitchenOrder => kitchenOrder.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (kitchenOrder == null) return CancelKitchenOrderResult.NotFound;
        
        if (!kitchenOrder.TransitionTo(KitchenStatus.Cancelled)) return CancelKitchenOrderResult.CannotCancel;

        await db.SaveChangesAsync(cancellationToken);
        return CancelKitchenOrderResult.Cancelled;
    }
}