namespace OrderService.Features.Orders.CreateOrder;

public record CreateOrderItemDto(int ProductId, string ProductName, decimal Quantity, decimal UnitPrice);