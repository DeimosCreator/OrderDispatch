using KitchenService.Models.Enities.Enums;
using MediatR;

namespace KitchenService.Features.KitchenOrder.PrepareKitchenOrder;

public record PrepareKitchenOrderCommand(int Id) : IRequest<PrepareKitchenOrderResult>;