using MediatR;
using OrderService.Models.Enities.Enums;

namespace OrderService.Features.Orders.CancelOrder;

public record CancelOrderCommand(int Id) : IRequest<CancelOrderResult>;