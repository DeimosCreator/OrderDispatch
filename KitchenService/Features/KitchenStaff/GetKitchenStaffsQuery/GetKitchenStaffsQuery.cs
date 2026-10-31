using KitchenService.Models.Dtos;
using MediatR;

namespace KitchenService.Features.KitchenStaff.GetKitchenStaffsQuery;

public record GetKitchenStaffsQuery(int KitchenId) : IRequest<List<KitchenStaffDto>>;