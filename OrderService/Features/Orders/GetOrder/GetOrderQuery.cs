using MediatR;
using OrderService.Models.Dtos;

namespace OrderService.Features.Orders.GetOrder;

public record GetOrderQuery(int Id) : IRequest<OrderDto?>;