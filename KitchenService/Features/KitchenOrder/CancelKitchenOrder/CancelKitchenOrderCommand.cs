using KitchenService.Models.Enities.Enums;
using MediatR;

namespace KitchenService.Features.KitchenOrder.CancelKitchenOrder;

public record CancelKitchenOrderCommand(int Id) : IRequest<CancelKitchenOrderResult>;