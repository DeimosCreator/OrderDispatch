using MediatR;
using OrderService.Models.Dtos;

namespace OrderService.Features.Orders.GetOrders;

public record GetOrdersQuery : IRequest<List<OrderDto>>;