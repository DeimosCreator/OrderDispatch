namespace KitchenService.Models.Dtos;

public record KitchenStaffDto(
    int Id,
    int KitchenId,
    string UserId
);