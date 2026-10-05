using OrderDispatch.Contracts.Enums;

namespace OrderDispatch.Contracts.Messaging.Events;

public record OrderCreatedEvent(
    int OrderId, 
    string CustomerId, 
    OrderStatus Status, 
    decimal TotalPrice, 
    DateTime CreatedAt
);