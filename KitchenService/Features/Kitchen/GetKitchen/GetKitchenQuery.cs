using KitchenService.Models.Dtos;
using MediatR;

namespace KitchenService.Features.Kitchen.GetKitchen;

public record GetKitchenQuery(int Id) : IRequest<KitchenDto?>;