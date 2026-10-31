using KitchenService.Data;
using KitchenService.Models.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KitchenService.Features.KitchenStaff.GetKitchenStaffsQuery;

public class GetKitchenStaffsQueryHandler(AppDbContext db) : IRequestHandler<GetKitchenStaffsQuery, List<KitchenStaffDto>>
{
    public async Task<List<KitchenStaffDto>> Handle(GetKitchenStaffsQuery request, CancellationToken cancellationToken)
    {
        var kitchenStaffs = await db.KitchenStaffs.Where(kitchenStaff => kitchenStaff.KitchenId == request.KitchenId)
            .ToListAsync(cancellationToken: cancellationToken);

        var kitchenStaffDtos = kitchenStaffs.Select(kitchenStaff =>
                new KitchenStaffDto(kitchenStaff.Id, kitchenStaff.KitchenId, kitchenStaff.UserId))
            .ToList();
        
        return kitchenStaffDtos;
    }
}