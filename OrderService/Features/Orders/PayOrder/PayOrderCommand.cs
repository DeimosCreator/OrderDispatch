using MediatR;
using OrderService.Models.Enities.Enums;

namespace OrderService.Features.Orders.PayOrder;

public record PayOrderCommand(int Id) : IRequest<PayOrderResult>;