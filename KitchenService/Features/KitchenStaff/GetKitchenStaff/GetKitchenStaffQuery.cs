using KitchenService.Models.Dtos;
using MediatR;

namespace KitchenService.Features.KitchenStaff.GetKitchenStaff;

public record GetKitchenStaffQuery(string UserId) : IRequest<KitchenStaffDto?>;