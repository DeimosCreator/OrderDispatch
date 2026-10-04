using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderDispatch.Contracts.Enums;
using OrderService.Data;
using OrderService.Models.Enities.Enums;

namespace OrderService.Features.Orders.PayOrder;

public class PayOrderCommandHandler(AppDbContext db) : IRequestHandler<PayOrderCommand, PayOrderResult>
{
    public async Task<PayOrderResult> Handle(PayOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await db.Orders.FirstOrDefaultAsync(order => order.Id == request.Id, cancellationToken: cancellationToken);

        if (order == null)
        {
            return PayOrderResult.NotFound;
        }
        
        if (!order.TransitionTo(OrderStatus.Paid)) return PayOrderResult.CannotPaid;
        
        await db.SaveChangesAsync(cancellationToken);
        return PayOrderResult.Paid;
    }
}