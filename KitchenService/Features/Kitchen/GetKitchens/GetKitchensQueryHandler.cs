using KitchenService.Data;
using KitchenService.Models.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KitchenService.Features.Kitchen.GetKitchens;

public class GetKitchensQueryHandler(AppDbContext db) : IRequestHandler<GetKitchensQuery, List<KitchenDto>>
{
    public async Task<List<KitchenDto>> Handle(GetKitchensQuery request, CancellationToken cancellationToken)
    {
        var kitchenDtos = await db.Kitchens
            .Select(kitchen =>
                new KitchenDto(kitchen.Id, kitchen.Name, kitchen.Capacity, kitchen.CurrentLoad, kitchen.IsActive))
            .ToListAsync(cancellationToken: cancellationToken);

        return kitchenDtos;
    }
}