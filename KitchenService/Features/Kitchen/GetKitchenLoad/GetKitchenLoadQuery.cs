using MediatR;

namespace KitchenService.Features.Kitchen.GetKitchenLoad;

public record GetKitchenLoadQuery(int Id) : IRequest<float?>;