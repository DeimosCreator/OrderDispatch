using OrderDispatch.Contracts.Enums;

namespace OrderDispatch.Contracts.Dtos;

public record OrderDto(
    int Id,
    string UserId,
    OrderStatus Status,
    decimal TotalPrice,
    DateTime CreatedAt,
    DateTime UpdatedAt
);