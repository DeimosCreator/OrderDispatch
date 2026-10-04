namespace OrderDispatch.Contracts.Dtos;

public record GetOrdersByIdsRequest(List<int> OrderIds);