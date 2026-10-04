using MediatR;
using OrderDispatch.Contracts.Dtos;

namespace KitchenService.Features.Kitchen.GetKitchenOrders;

public record GetKitchenOrdersQuery(int Id) : IRequest<List<OrderDto>>;