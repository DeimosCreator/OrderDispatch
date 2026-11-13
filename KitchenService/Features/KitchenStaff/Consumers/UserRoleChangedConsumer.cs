using KitchenService.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderDispatch.Contracts.Constants;
using OrderDispatch.Contracts.Messaging.Events.Users;

namespace KitchenService.Features.KitchenStaff.Consumers;

public class UserRoleChangedConsumer(AppDbContext db) : IConsumer<UserRoleChangedEvent>
{
    public async Task Consume(ConsumeContext<UserRoleChangedEvent> context)
    {
        var ct = context.CancellationToken;
        var msg = context.Message;

        if (msg.Role is not (AppRoles.Chef or AppRoles.BranchManager))
            return;

        if (await db.KitchenStaffs.AnyAsync(ks => ks.UserId == msg.UserId, ct))
            return;

        var kitchenWithMinStaffCount = await db.Kitchens
            .Where(kitchen => kitchen.IsActive)
            .OrderBy(kitchen => kitchen.KitchenStaffs.Count)
            .FirstOrDefaultAsync(ct);

        var kitchenStaff = new Models.Entities.KitchenStaff
        {
            UserId = msg.UserId,
            KitchenId = kitchenWithMinStaffCount?.Id
        };

        db.KitchenStaffs.Add(kitchenStaff);
        await db.SaveChangesAsync(ct);
    }
}