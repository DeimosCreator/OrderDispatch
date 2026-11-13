using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderDispatch.Contracts.Enums;
using OrderDispatch.Contracts.Messaging.Events.Orders;
using OrderService.Data;
using OrderService.Models.Entities.Enums;

namespace OrderService.Features.Orders.CancelOrder;

public class CancelOrderCommandHandler(AppDbContext db, IPublishEndpoint publishEndpoint)
    : IRequestHandler<CancelOrderCommand, CancelOrderResult>
{
    public async Task<CancelOrderResult> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await db.Orders.FirstOrDefaultAsync(
            order => order.UserId == request.UserId && order.Id == request.Id, cancellationToken);

        if (order == null) return CancelOrderResult.NotFound;

        if (!order.TransitionTo(OrderStatus.Cancelled)) return CancelOrderResult.CannotCancel;

        await db.SaveChangesAsync(cancellationToken);

        await publishEndpoint.Publish(
            new OrderStatusChangedEvent(order.Id, order.Status, order.UpdatedAt),
            cancellationToken);

        return CancelOrderResult.Cancelled;
    }
}