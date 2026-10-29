using KitchenService.Models.Enities;

namespace KitchenService.Models.Entities;

public class Kitchen
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public float CurrentLoad { get; set; }
    public bool IsActive { get; set; }

    public List<KitchenOrder> KitchenOrders { get; set; } = [];
}