using OrderDispatch.Contracts.Dtos;

namespace KitchenService.Features.Kitchen.Clients.OrderService;

public class OrderHttpClient(HttpClient httpClient) : IOrderHttpClient
{
    public async Task<List<OrderDto>> GetOrders(List<int> orderIds, CancellationToken cancellationToken)
    {
        if (orderIds.Count == 0) return [];

        try
        {
            var response = await httpClient.PostAsJsonAsync("api/orders/batch", 
                new GetOrdersByIdsRequest(orderIds), cancellationToken);
        
            response.EnsureSuccessStatusCode();

            var orders = await response.Content.ReadFromJsonAsync<List<OrderDto>>(cancellationToken: cancellationToken);
            return orders ?? [];
        }
        catch (Exception)
        {
            return [];
        }
    }
}