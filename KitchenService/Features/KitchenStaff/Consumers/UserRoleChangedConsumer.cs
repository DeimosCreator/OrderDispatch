using KitchenService.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderDispatch.Contracts.Messaging.Events.Users;

namespace KitchenService.Features.KitchenStaff.Consumers;

public class UserRoleChangedConsumer(AppDbContext db) : IConsumer<UserRoleChangedEvent>
{
    public async Task Consume(ConsumeContext<UserRoleChangedEvent> context)
    {
        var kitchenWithMinStaffCount = await db.Kitchens
            .Where(kitchen => kitchen.IsActive)
            .OrderBy(kitchen => kitchen.KitchenStaffs.Count)
            .FirstOrDefaultAsync();

        var kitchenStaff = new Models.Entities.KitchenStaff
        {
            UserId = context.Message.UserId,
            KitchenId = kitchenWithMinStaffCount?.Id 
        };
        
        db.KitchenStaffs.Add(kitchenStaff);
        await db.SaveChangesAsync();
    }
}