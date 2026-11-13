using System.ComponentModel.DataAnnotations;
using MediatR;
using OrderDispatch.Contracts.Dtos.Orders;

namespace OrderService.Features.Orders.CreateOrder;

public record CreateOrderCommand(
    string UserId,
    List<CreateOrderItemDto> Items
) : IRequest<OrderDto>;