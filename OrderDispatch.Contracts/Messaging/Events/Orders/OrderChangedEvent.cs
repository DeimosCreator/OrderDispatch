namespace OrderDispatch.Contracts.Messaging.Events.Orders;

public record OrderChangedEvent(int OrderId, DateTime UpdatedAt);