using KitchenService.Data;
using KitchenService.Models.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KitchenService.Features.KitchenStaff.GetKitchen;

public class GetKitchenQueryHandler(AppDbContext db) : IRequestHandler<GetKitchenQuery, KitchenDto?>
{
    public async Task<KitchenDto?> Handle(GetKitchenQuery request, CancellationToken cancellationToken)
    {
        var kitchen = await db.Kitchens
            .AsNoTracking()
            .Where(kitchen => kitchen.KitchenStaffs.Any(kitchenStaff => kitchenStaff.UserId == request.UserId))
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (kitchen == null)
            return null;

        var kitchenDto = new KitchenDto(kitchen.Id, kitchen.Name, kitchen.Capacity, kitchen.CurrentLoad,
            kitchen.IsActive);
        return kitchenDto;
    }
}