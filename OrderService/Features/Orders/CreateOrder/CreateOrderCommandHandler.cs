using MassTransit;
using MediatR;
using OrderDispatch.Contracts.Dtos.Orders;
using OrderDispatch.Contracts.Messaging.Events.Orders;
using OrderService.Data;
using OrderService.Models.Entities;

namespace OrderService.Features.Orders.CreateOrder;

public class CreateOrderCommandHandler(AppDbContext db, IPublishEndpoint publishEndpoint) : IRequestHandler<CreateOrderCommand, OrderDto>
{
    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            UserId = request.UserId,
            TotalPrice = request.Items.Select(item => item.Quantity * item.UnitPrice).Sum(),
            OrderItems = request.Items.Select(item => new OrderItem
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            }).ToList()
        };
        
        db.Orders.Add(order);
        await db.SaveChangesAsync(cancellationToken);

        await publishEndpoint.Publish(new OrderCreatedEvent(
                order.Id, order.UserId, order.Status, order.TotalPrice, order.CreatedAt, request.Items
                    .Select(item => new OrderItemDto(item.ProductId, item.ProductName, item.Quantity, item.UnitPrice))
                    .ToList()),
            cancellationToken);

        var orderDto = new OrderDto(order.Id, order.UserId, order.Status, order.TotalPrice, order.CreatedAt,
            order.UpdatedAt);
        
        return orderDto;
    }
}