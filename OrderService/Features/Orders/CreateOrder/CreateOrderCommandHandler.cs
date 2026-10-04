using MediatR;
using OrderDispatch.Contracts.Dtos;
using OrderService.Data;
using OrderService.Models.Enities;

namespace OrderService.Features.Orders.CreateOrder;

public class CreateOrderCommandHandler(AppDbContext db) : IRequestHandler<CreateOrderCommand, OrderDto>
{
    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            CustomerId = request.CustomerId,
            TotalPrice = request.TotalPrice
        };

        db.Orders.Add(order);
        await db.SaveChangesAsync(cancellationToken);

        var orderDto = new OrderDto(order.Id, order.CustomerId, order.Status, order.TotalPrice, order.CreatedAt,
            order.UpdatedAt);
        
        return orderDto;
    }
}