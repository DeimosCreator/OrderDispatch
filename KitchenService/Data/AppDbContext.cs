using KitchenService.Models.Enities;
using Microsoft.EntityFrameworkCore;

namespace KitchenService.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Kitchen> Kitchens => Set<Kitchen>();
    public DbSet<KitchenOrder> KitchenOrders => Set<KitchenOrder>();
}