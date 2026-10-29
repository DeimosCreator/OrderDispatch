using MediatR;
using OrderService.Models.Entities.Enums;

namespace OrderService.Features.Orders.CancelOrder;

public record CancelOrderCommand(int Id, string UserId) : IRequest<CancelOrderResult>;