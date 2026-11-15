using System.ComponentModel.DataAnnotations;
using MediatR;
using OrderDispatch.Contracts.Dtos.Orders;

namespace OrderService.Features.Orders.CreateOrder;

public record CreateOrderCommand(
    string UserId,
    [MinLength(1)] List<CreateOrderItemDto> Items
) : IRequest<OrderDto>;