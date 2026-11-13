using System.ComponentModel.DataAnnotations;
using KitchenService.Models.Dtos;
using MediatR;

namespace KitchenService.Features.Kitchen.CreateKitchen;

public record CreateKitchenCommand(
    [Required, MinLength(1)] string Name,
    [Range(1, int.MaxValue)] int Capacity = 5,
    bool IsActive = true
) : IRequest<KitchenDto>;