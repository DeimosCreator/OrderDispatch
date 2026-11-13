using KitchenService.Models.Dtos;
using MediatR;

namespace KitchenService.Features.Kitchen.GetKitchenOrders;

public record GetKitchenOrdersQuery(int Id) : IRequest<List<KitchenOrderDto>>;