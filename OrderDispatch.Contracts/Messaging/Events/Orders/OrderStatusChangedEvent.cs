using OrderDispatch.Contracts.Enums;

namespace OrderDispatch.Contracts.Messaging.Events.Orders;

public record OrderStatusChangedEvent(
    int OrderId, 
    OrderStatus NewStatus, 
    DateTime UpdatedAt
);