using OrderService.Models.Enities.Enums;

namespace OrderService.Models.Dtos;

public record OrderDto(
    int Id,
    string CustomerId,
    OrderStatus Status,
    decimal TotalPrice,
    DateTime CreatedAt,
    DateTime UpdatedAt
);