using MediatR;
using OrderService.Models.Enities.Enums;

namespace OrderService.Features.Orders.GetOrderStatus;

public record GetOrderStatusQuery(int Id) : IRequest<OrderStatus?>;