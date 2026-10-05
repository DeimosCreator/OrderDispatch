using KitchenService.Models.Enities.Enums;
using MediatR;

namespace KitchenService.Features.KitchenOrder.ReadyKitchenOrder;

public record ReadyKitchenOrderCommand(int Id) : IRequest<ReadyKitchenOrderResult>;