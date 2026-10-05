using OrderDispatch.Contracts.Enums;

namespace KitchenService.Models.Enities;

public class OrderReadModel
{
    public int Id { get; set; } 
    public string CustomerId { get; set; } = string.Empty;
    public OrderStatus Status { get; set; }
    public decimal TotalPrice { get; set; }
}