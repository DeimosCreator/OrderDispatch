using OrderService.Models.Enities;

namespace OrderService.Models.Dtos;

public record OrderDto(
    int Id,
    int CustomerId,
    OrderStatus Status,
    decimal TotalPrice,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record CreateOrderDto(
    int Id,
    int CustomerId,
    decimal TotalPrice
);

public record UpdateOrderDto(
    int Id,
    int CustomerId,
    decimal TotalPrice
);