using KitchenService.Models.Enities.Enums;

namespace KitchenService.Models.Dtos;

public record KitchenOrderDto(
    int Id,
    int OrderId,
    int KitchenId,
    KitchenStatus Status,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime StartedAt,
    DateTime ReadyAt);