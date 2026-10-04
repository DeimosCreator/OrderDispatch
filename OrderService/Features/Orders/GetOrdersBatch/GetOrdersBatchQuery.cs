using MediatR;
using OrderDispatch.Contracts.Dtos;

namespace OrderService.Features.Orders.GetOrdersBatch;

public record GetOrdersBatchQuery(List<int> OrderIds) : IRequest<List<OrderDto>>;