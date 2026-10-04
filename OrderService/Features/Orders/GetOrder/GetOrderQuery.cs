using MediatR;
using OrderDispatch.Contracts.Dtos;

namespace OrderService.Features.Orders.GetOrder;

public record GetOrderQuery(int Id) : IRequest<OrderDto?>;