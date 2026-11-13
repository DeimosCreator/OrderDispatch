namespace OrderDispatch.Contracts.Dtos.Orders;

public record OrderItemDto(int ProductId, string ProductName, decimal Quantity, decimal UnitPrice);