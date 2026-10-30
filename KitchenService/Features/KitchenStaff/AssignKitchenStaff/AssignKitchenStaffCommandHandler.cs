using KitchenService.Data;
using KitchenService.Models.Entities.Enums;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderDispatch.Contracts.Constants;
using OrderDispatch.Contracts.Messaging.Commands;

namespace KitchenService.Features.KitchenStaff.AssignKitchenStaff;

public class AssignKitchenStaffCommandHandler(
    AppDbContext db,
    IRequestClient<GetUserRolesRequest> requestClient)
    : IRequestHandler<AssignKitchenStaffCommand, AssignKitchenStaffStatus>
{
    public async Task<AssignKitchenStaffStatus> Handle(AssignKitchenStaffCommand request, CancellationToken cancellationToken)
    {
        var kitchen = await db.Kitchens.Where(kitchen => kitchen.Id == request.KitchenId)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (kitchen == null)
            return AssignKitchenStaffStatus.KitchenNotFound;
        
        var assignStaffUserRole =
            (await requestClient.GetResponse<UserRoleResponse>(new GetUserRolesRequest(request.AssignStaffUserId),
                cancellationToken)).Message.Role;

        if (assignStaffUserRole == null)
            return AssignKitchenStaffStatus.UserNotFound;

        if (assignStaffUserRole is not (AppRoles.Chef or AppRoles.BranchManager))
            return AssignKitchenStaffStatus.CannotAssignKitchenStaff;
        
        var userRole =
            (await requestClient.GetResponse<UserRoleResponse>(new GetUserRolesRequest(request.UserId),
                cancellationToken)).Message.Role;

        if (!AppRoles.IsHigher([userRole], [assignStaffUserRole]))
            return AssignKitchenStaffStatus.CannotAssignKitchenStaff;

        var kitchenStaff = await db.KitchenStaffs
            .Where(kitchenStaff => kitchenStaff.UserId == request.AssignStaffUserId)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        
        if (kitchenStaff == null)
            return AssignKitchenStaffStatus.UserNotFound;

        kitchenStaff.KitchenId = request.KitchenId;
        await db.SaveChangesAsync(cancellationToken);

        return AssignKitchenStaffStatus.AssignKitchenStaff;
    }
}