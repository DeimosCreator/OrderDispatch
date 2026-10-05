using KitchenService.Models.Enities.Enums;
using MediatR;

namespace KitchenService.Features.KitchenOrder.AcceptKitchenOrder;

public record AcceptKitchenOrderCommand(int Id) : IRequest<AcceptKitchenOrderResult>;