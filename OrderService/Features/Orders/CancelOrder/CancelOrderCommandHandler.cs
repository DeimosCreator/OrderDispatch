using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderDispatch.Contracts.Enums;
using OrderService.Data;
using OrderService.Models.Enities.Enums;

namespace OrderService.Features.Orders.CancelOrder;

public class CancelOrderCommandHandler(AppDbContext db) : IRequestHandler<CancelOrderCommand, CancelOrderResult>
{
    public async Task<CancelOrderResult> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await db.Orders.FirstOrDefaultAsync(order => order.Id == request.Id, cancellationToken: cancellationToken);

        if (order == null)
        {
            return CancelOrderResult.NotFound;
        }

        if (!order.TransitionTo(OrderStatus.Cancelled)) return CancelOrderResult.CannotCancel;
        
        await db.SaveChangesAsync(cancellationToken);
        return CancelOrderResult.Cancelled;
    }
}