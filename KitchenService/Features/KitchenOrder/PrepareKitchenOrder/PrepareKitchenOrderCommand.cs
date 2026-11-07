using KitchenService.Models.Entities.Enums;
using MediatR;

namespace KitchenService.Features.KitchenOrder.PrepareKitchenOrder;

public record PrepareKitchenOrderCommand(int Id) : IRequest<PrepareKitchenOrderResult>;