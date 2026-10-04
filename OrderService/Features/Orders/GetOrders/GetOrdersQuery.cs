using MediatR;
using OrderDispatch.Contracts.Dtos;

namespace OrderService.Features.Orders.GetOrders;

public record GetOrdersQuery : IRequest<List<OrderDto>>;