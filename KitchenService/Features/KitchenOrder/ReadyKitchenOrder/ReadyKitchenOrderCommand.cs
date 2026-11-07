using KitchenService.Models.Entities.Enums;
using MediatR;

namespace KitchenService.Features.KitchenOrder.ReadyKitchenOrder;

public record ReadyKitchenOrderCommand(int Id) : IRequest<ReadyKitchenOrderResult>;