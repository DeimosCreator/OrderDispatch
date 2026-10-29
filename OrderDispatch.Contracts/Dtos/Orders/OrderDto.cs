using OrderDispatch.Contracts.Enums;

namespace OrderDispatch.Contracts.Dtos.Orders;

public record OrderDto(
    int Id,
    string UserId,
    OrderStatus Status,
    decimal TotalPrice,
    DateTime CreatedAt,
    DateTime UpdatedAt
);