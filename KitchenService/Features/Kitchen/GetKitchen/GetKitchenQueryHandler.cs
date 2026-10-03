using KitchenService.Data;
using KitchenService.Models.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KitchenService.Features.Kitchen.GetKitchen;

public class GetKitchenQueryHandler(AppDbContext db) : IRequestHandler<GetKitchenQuery, KitchenDto?>
{
    public async Task<KitchenDto?> Handle(GetKitchenQuery request, CancellationToken cancellationToken)
    {
        var kitchen = await db.Kitchens
            .FirstOrDefaultAsync(kitchen => kitchen.Id == request.Id, cancellationToken: cancellationToken);

        if (kitchen == null)
        {
            return null;
        }

        var kitchenDto = new KitchenDto(kitchen.Id, kitchen.Name, kitchen.Capacity, kitchen.CurrentLoad,
            kitchen.IsActive);

        return kitchenDto;
    }
}