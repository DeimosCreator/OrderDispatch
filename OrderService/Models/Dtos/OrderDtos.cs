using System.ComponentModel.DataAnnotations;
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
    [property: Range(0, int.MaxValue)] int CustomerId,
    [property: Range(0.01, int.MaxValue)] decimal TotalPrice
);

public record UpdateOrderDto(
    [property: Range(0, int.MaxValue)] int Id,
    [property: Range(0, int.MaxValue)] int CustomerId,
    decimal TotalPrice
);