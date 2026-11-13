using KitchenService.Data;
using KitchenService.Models.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KitchenService.Features.KitchenStaff.GetKitchenStaff;

public class GetKitchenStaffQueryHandler(AppDbContext db) : IRequestHandler<GetKitchenStaffQuery, KitchenStaffDto?>
{
    public async Task<KitchenStaffDto?> Handle(GetKitchenStaffQuery request, CancellationToken cancellationToken)
    {
        var kitchenStaff = await db.KitchenStaffs.Where(kitchenStaff => kitchenStaff.UserId == request.UserId)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (kitchenStaff == null)
            return null;

        var kitchenStaffDto = new KitchenStaffDto(kitchenStaff.Id, kitchenStaff.KitchenId, kitchenStaff.UserId);
        return kitchenStaffDto;
    }
}