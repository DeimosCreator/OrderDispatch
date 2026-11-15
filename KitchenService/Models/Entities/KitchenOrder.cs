using KitchenService.Models.Entities.Enums;
using KitchenService.StateMachine;

namespace KitchenService.Models.Entities;

public class KitchenOrder
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int? KitchenId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public KitchenStatus Status { get; private set; } = KitchenStatus.Queued;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime StartedAt { get; set; }
    public DateTime ReadyAt { get; set; }

    public List<KitchenOrderItem> KitchenOrderItems { get; set; } = [];
    
    public bool TransitionTo(KitchenStatusInput nextStatus)
    {
        if (!KitchenStateMachine.EnsureCanTransition(Status, nextStatus, out var resolvedStatus)) return false;
        
        Status = resolvedStatus;
        UpdatedAt = DateTime.UtcNow;
        return true;
    }
}