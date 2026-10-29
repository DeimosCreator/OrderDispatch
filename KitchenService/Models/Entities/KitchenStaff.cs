namespace KitchenService.Models.Entities;

public class KitchenStaff
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty; 
    public int? KitchenId { get; set; }
}