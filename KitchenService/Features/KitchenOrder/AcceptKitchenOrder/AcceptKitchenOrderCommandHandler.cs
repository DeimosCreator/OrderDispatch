using KitchenService.Data;
using KitchenService.Models.Enities.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KitchenService.Features.KitchenOrder.AcceptKitchenOrder;

public class AcceptKitchenOrderCommandHandler(AppDbContext db) : IRequestHandler<AcceptKitchenOrderCommand, AcceptKitchenOrderResult>
{
    public async Task<AcceptKitchenOrderResult> Handle(AcceptKitchenOrderCommand request, CancellationToken cancellationToken)
    {
        var kitchenOrder = await db.KitchenOrders.Where(kitchenOrder => kitchenOrder.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (kitchenOrder == null) return AcceptKitchenOrderResult.NotFound;
        
        if (!kitchenOrder.TransitionTo(KitchenStatus.Accepted)) return AcceptKitchenOrderResult.CannotAccept;

        await db.SaveChangesAsync(cancellationToken);
        return AcceptKitchenOrderResult.Accepted;
    }
}