using MediatR;
using OrderService.Models.Entities.Enums;

namespace OrderService.Features.Orders.PayOrder;

public record PayOrderCommand(int Id, string UserId) : IRequest<PayOrderResult>;