using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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

public record CreateOrderDto(
    string CustomerId,
    [Range(0.01, int.MaxValue)] decimal TotalPrice = 100
);

public record UpdateOrderDto(
    [Range(0, int.MaxValue)] int Id,
    string CustomerId,
    [Range(0.01, int.MaxValue)] decimal TotalPrice = 100
);