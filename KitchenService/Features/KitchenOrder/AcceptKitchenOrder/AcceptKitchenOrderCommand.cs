using KitchenService.Models.Entities.Enums;
using MediatR;

namespace KitchenService.Features.KitchenOrder.AcceptKitchenOrder;

public record AcceptKitchenOrderCommand(int Id, string UserId) : IRequest<AcceptKitchenOrderResult>;