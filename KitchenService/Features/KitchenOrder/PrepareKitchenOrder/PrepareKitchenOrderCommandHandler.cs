using KitchenService.Data;
using KitchenService.Models.Enities.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KitchenService.Features.KitchenOrder.PrepareKitchenOrder;

public class PrepareKitchenOrderCommandHandler(AppDbContext db) 
    : IRequestHandler<PrepareKitchenOrderCommand, PrepareKitchenOrderResult>
{
    public async Task<PrepareKitchenOrderResult> Handle(PrepareKitchenOrderCommand request, CancellationToken cancellationToken)
    {
        var kitchenOrder = await db.KitchenOrders.Where(kitchenOrder => kitchenOrder.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (kitchenOrder == null) return PrepareKitchenOrderResult.NotFound;
        
        if (!kitchenOrder.TransitionTo(KitchenStatus.Preparing)) return PrepareKitchenOrderResult.CannotPrepare;

        await db.SaveChangesAsync(cancellationToken);
        return PrepareKitchenOrderResult.Preparing;
    }
}