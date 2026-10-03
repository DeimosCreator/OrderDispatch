using KitchenService.Models.Enities.Enums;

namespace KitchenService.Models.Enities;

public class KitchenOrder
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int KitchenId { get; set; }
    public KitchenStatus Status { get; private set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime StartedAt { get; set; }
    public DateTime ReadyAt { get; set; }
}