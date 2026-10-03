using OrderService.Models.Enities.Enums;
using OrderService.StateMachine;

namespace OrderService.Models.Enities;

public class Order
{
    public int Id { get; set; }
    public string CustomerId { get; set; } = string.Empty;
    public OrderStatus Status { get; private set; } = OrderStatus.Created;
    public decimal TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public List<OrderItem> OrderItems { get; set; } = [];
    
    public bool TransitionTo(OrderStatus nextStatus)
    {
        if (!OrderStateMachine.EnsureCanTransition(Status, nextStatus)) return false;
        
        Status = nextStatus;
        UpdatedAt = DateTime.UtcNow;
        return true;
    }
}