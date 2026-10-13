using MediatR;
using OrderDispatch.Contracts.Dtos;

namespace OrderService.Features.Orders.GetOrders;

public record GetOrdersQuery(string UserId) : IRequest<List<OrderDto>>;