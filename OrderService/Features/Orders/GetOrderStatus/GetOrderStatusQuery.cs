using MediatR;
using OrderDispatch.Contracts.Enums;

namespace OrderService.Features.Orders.GetOrderStatus;

public record GetOrderStatusQuery(int Id, string UserId) : IRequest<OrderStatus?>;