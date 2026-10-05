using KitchenService.Data;
using KitchenService.Models.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KitchenService.Features.KitchenOrder.GetKitchenOrder;

public class GetKitchenOrderQueryHandler(AppDbContext db) : IRequestHandler<GetKitchenOrderQuery, KitchenOrderDto?>
{
    public async Task<KitchenOrderDto?> Handle(GetKitchenOrderQuery request, CancellationToken cancellationToken)
    {
        var kitchenOrder = await db.KitchenOrders.Where(kitchenOrder => kitchenOrder.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (kitchenOrder == null)
        {
            return null;
        }

        var kitchenOrderDto = new KitchenOrderDto(kitchenOrder.Id, kitchenOrder.OrderId, kitchenOrder.KitchenId,
            kitchenOrder.Status, kitchenOrder.CreatedAt, kitchenOrder.UpdatedAt, kitchenOrder.StartedAt,
            kitchenOrder.ReadyAt);

        return kitchenOrderDto;
    }
}