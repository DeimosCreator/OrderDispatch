using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderDispatch.Contracts.Enums;
using OrderDispatch.Contracts.Messaging.Events.Orders;
using OrderService.Data;
using OrderService.Models.Entities.Enums;

namespace OrderService.Features.Orders.PayOrder;

public class PayOrderCommandHandler(AppDbContext db, IPublishEndpoint publishEndpoint)
    : IRequestHandler<PayOrderCommand, PayOrderResult>
{
    public async Task<PayOrderResult> Handle(PayOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await db.Orders.FirstOrDefaultAsync(
            order => order.UserId == request.UserId && order.Id == request.Id, cancellationToken);

        if (order == null) return PayOrderResult.NotFound;

        if (!order.TransitionTo(OrderStatus.PaymentPending)) return PayOrderResult.CannotPaid;
        await db.SaveChangesAsync(cancellationToken);
        
        //TODO: Здесь должна быть реальная оплата

        if (!order.TransitionTo(OrderStatus.Paid)) return PayOrderResult.CannotPaid;
        await db.SaveChangesAsync(cancellationToken);

        await publishEndpoint.Publish(new OrderPaidEvent(order.Id, order.UpdatedAt),
            cancellationToken);

        return PayOrderResult.Paid;
    }
}