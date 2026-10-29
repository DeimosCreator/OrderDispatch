using MediatR;
using OrderDispatch.Contracts.Dtos;
using OrderDispatch.Contracts.Dtos.Orders;

namespace OrderService.Features.Orders.GetOrders;

public record GetOrdersQuery(string UserId) : IRequest<List<OrderDto>>;