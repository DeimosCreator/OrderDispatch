using OrderService.Models.Dtos;
using OrderService.Models.Enities;
using OrderService.Models.Enities.Enums;

namespace OrderService.Services.Interfaces;

public interface IOrderService
{
    public Task<List<OrderDto>> GetOrders();
    public Task<OrderDto> CreateOrder(CreateOrderDto createOrderDto);
    public Task<OrderDto?> GetOrder(int id);
    public Task<OrderStatus?> GetOrderStatus(int id);
    public Task<PayOrderResult> PayOrder(int id);
    public Task<CancelOrderResult> CancelOrder(int id);
}