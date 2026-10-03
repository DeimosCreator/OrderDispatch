using KitchenService.Models.Dtos;
using MediatR;

namespace KitchenService.Features.Kitchen.GetKitchens;

public record GetKitchensQuery : IRequest<List<KitchenDto>>;