using MassTransit;
using MediatR;
using OrderDispatch.Contracts.Dtos;
using OrderDispatch.Contracts.Messaging.Events;
using OrderService.Data;
using OrderService.Models.Enities;

namespace OrderService.Features.Orders.CreateOrder;

public class CreateOrderCommandHandler(AppDbContext db, IPublishEndpoint publishEndpoint) : IRequestHandler<CreateOrderCommand, OrderDto>
{
    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            UserId = request.UserId,
            TotalPrice = request.TotalPrice
        };

        db.Orders.Add(order);
        await db.SaveChangesAsync(cancellationToken);

        await publishEndpoint.Publish(new OrderCreatedEvent(
            order.Id, order.UserId, order.Status, order.TotalPrice, order.CreatedAt), cancellationToken);

        var orderDto = new OrderDto(order.Id, order.UserId, order.Status, order.TotalPrice, order.CreatedAt,
            order.UpdatedAt);
        
        return orderDto;
    }
}