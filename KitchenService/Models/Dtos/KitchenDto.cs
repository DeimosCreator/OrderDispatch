namespace KitchenService.Models.Dtos;

public record KitchenDto(
    int Id, 
    string Name,
    int Capacity,
    float CurrentLoad,
    bool IsActive);