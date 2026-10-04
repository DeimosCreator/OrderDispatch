using OrderDispatch.Contracts.Dtos;

namespace KitchenService.Features.Kitchen.Clients.OrderService;

public interface IOrderHttpClient
{
    public Task<List<OrderDto>> GetOrders(List<int> orderIds, CancellationToken cancellationToken);
}