namespace OrderDispatch.Contracts.Dtos.Orders;

public record GetOrdersByIdsRequest(List<int> OrderIds);