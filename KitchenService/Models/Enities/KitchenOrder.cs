using KitchenService.Models.Enities.Enums;
using KitchenService.StateMachine;
using OrderDispatch.Contracts.Enums;

namespace KitchenService.Models.Enities;

public class KitchenOrder
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int KitchenId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public KitchenStatus Status { get; private set; } = KitchenStatus.Queued;
    public OrderStatus GlobalOrderStatus { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime StartedAt { get; set; }
    public DateTime ReadyAt { get; set; }
    
    public bool TransitionTo(KitchenStatus nextStatus)
    {
        if (!KitchenStateMachine.EnsureCanTransition(Status, nextStatus)) return false;
        
        Status = nextStatus;
        UpdatedAt = DateTime.UtcNow;
        return true;
    }
}