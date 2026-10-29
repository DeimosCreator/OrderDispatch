using MediatR;
using OrderDispatch.Contracts.Dtos;
using OrderDispatch.Contracts.Dtos.Orders;

namespace OrderService.Features.Orders.GetOrder;

public record GetOrderQuery(int Id, string UserId) : IRequest<OrderDto?>;