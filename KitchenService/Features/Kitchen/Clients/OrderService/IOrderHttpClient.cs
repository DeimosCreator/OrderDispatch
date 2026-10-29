using OrderDispatch.Contracts.Dtos;
using OrderDispatch.Contracts.Dtos.Orders;

namespace KitchenService.Features.Kitchen.Clients.OrderService;

public interface IOrderHttpClient
{
    public Task<List<OrderDto>> GetOrders(List<int> orderIds, CancellationToken cancellationToken);
}