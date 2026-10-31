using KitchenService.Models.Dtos;
using MediatR;

namespace KitchenService.Features.KitchenStaff.GetKitchen;

public record GetKitchenQuery(string UserId) : IRequest<KitchenDto?>;