using KitchenService.Models.Entities.Enums;
using MediatR;

namespace KitchenService.Features.KitchenStaff.AssignKitchenStaff;

public record AssignKitchenStaffCommand(int KitchenId, string UserId, string AssignStaffUserId)
    : IRequest<AssignKitchenStaffStatus>;