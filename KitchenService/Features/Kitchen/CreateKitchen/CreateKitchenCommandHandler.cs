using KitchenService.Data;
using KitchenService.Models.Dtos;
using MediatR;

namespace KitchenService.Features.Kitchen.CreateKitchen;

public class CreateKitchenCommandHandler(AppDbContext db) : IRequestHandler<CreateKitchenCommand, KitchenDto>
{
    public async Task<KitchenDto> Handle(CreateKitchenCommand request, CancellationToken cancellationToken)
    {
        var kitchen = new Models.Enities.Kitchen
        {
            Name = request.Name,
            Capacity = request.Capacity,
            CurrentLoad = request.CurrentLoad,
            IsActive = request.IsActive
        };

        db.Kitchens.Add(kitchen);
        await db.SaveChangesAsync(cancellationToken);

        var kitchenDto = new KitchenDto(kitchen.Id, kitchen.Name, kitchen.Capacity, kitchen.CurrentLoad,
            kitchen.IsActive);

        return kitchenDto;
    }
}