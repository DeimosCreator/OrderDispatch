using KitchenService.Models.Dtos;
using MediatR;

namespace KitchenService.Features.KitchenOrder.GetKitchenOrder;

public record GetKitchenOrderQuery(int Id) : IRequest<KitchenOrderDto?>;