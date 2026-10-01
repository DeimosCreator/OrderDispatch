using OrderService.Models.Dtos;
using OrderService.Models.Enities;

namespace OrderService.Services.Interfaces;

public interface IOrderService
{
    public Task<List<OrderDto>> GetOrders();
    public Task<OrderDto> CreateOrder(CreateOrderDto createOrderDto);
    public Task<OrderDto?> GetOrder(int id);
    public Task<OrderStatus?> GetOrderStatus(int id);
    public Task<OrderStatus?> PayOrder(int id);
    public Task<OrderStatus?> CancelOrder(int id);
}