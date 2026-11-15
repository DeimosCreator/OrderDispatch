using KitchenService.Data;
using KitchenService.Models.Entities.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderDispatch.Contracts.Enums;

namespace KitchenService.Features.KitchenOrder.PrepareKitchenOrder;

public class PrepareKitchenOrderCommandHandler(AppDbContext db) 
    : IRequestHandler<PrepareKitchenOrderCommand, PrepareKitchenOrderResult>
{
    public async Task<PrepareKitchenOrderResult> Handle(PrepareKitchenOrderCommand request, CancellationToken cancellationToken)
    {
        var kitchenOrder = await db.KitchenOrders.Where(kitchenOrder => kitchenOrder.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (kitchenOrder == null) return PrepareKitchenOrderResult.NotFound;
        
        if (!kitchenOrder.TransitionTo(KitchenStatus.Preparing) && kitchenOrder.GlobalOrderStatus != OrderStatus.Paid) 
            return PrepareKitchenOrderResult.CannotPrepare;

        kitchenOrder.UpdatedAt = DateTime.UtcNow;
        
        await db.SaveChangesAsync(cancellationToken);
        return PrepareKitchenOrderResult.Preparing;
    }
}