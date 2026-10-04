using KitchenService.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KitchenService.Features.Kitchen.GetKitchenLoad;

public class GetKitchenLoadQueryHandler(AppDbContext db) : IRequestHandler<GetKitchenLoadQuery, float?>
{
    public async Task<float?> Handle(GetKitchenLoadQuery request, CancellationToken cancellationToken)
    {
        var kitchen = await db.Kitchens
            .FirstOrDefaultAsync(kitchen => kitchen.Id == request.Id, cancellationToken: cancellationToken);

        var currentLoad = kitchen?.CurrentLoad;
        return currentLoad;
    }
}