using OrderDispatch.Contracts.Enums;

namespace OrderDispatch.Contracts.Messaging.Events;

public record OrderCreatedEvent(
    int OrderId, 
    string UserId, 
    OrderStatus Status, 
    decimal TotalPrice, 
    DateTime CreatedAt
);