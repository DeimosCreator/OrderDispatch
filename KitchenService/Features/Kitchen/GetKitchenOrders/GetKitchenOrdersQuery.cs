using MediatR;
using OrderDispatch.Contracts.Dtos;
using OrderDispatch.Contracts.Dtos.Orders;

namespace KitchenService.Features.Kitchen.GetKitchenOrders;

public record GetKitchenOrdersQuery(int Id) : IRequest<List<OrderDto>>;