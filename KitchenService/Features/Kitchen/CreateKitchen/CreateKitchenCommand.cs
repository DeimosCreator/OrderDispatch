using System.ComponentModel.DataAnnotations;
using KitchenService.Models.Dtos;
using MediatR;

namespace KitchenService.Features.Kitchen.CreateKitchen;

public record CreateKitchenCommand(
    string Name,
    [Range(0, int.MaxValue)] int Capacity = 5,
    [Range(0, 1)] float CurrentLoad = 0f,
    bool IsActive = true
) : IRequest<KitchenDto>;     