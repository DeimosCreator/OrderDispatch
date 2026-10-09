using System.ComponentModel.DataAnnotations;
using MediatR;
using OrderDispatch.Contracts.Dtos;

namespace OrderService.Features.Orders.CreateOrder;

public record CreateOrderCommand(
    string UserId,
    [Range(0.01, int.MaxValue)] decimal TotalPrice = 100
) : IRequest<OrderDto>;