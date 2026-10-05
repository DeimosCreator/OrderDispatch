using KitchenService.Models.Enities;
using Microsoft.EntityFrameworkCore;

namespace KitchenService.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Kitchen> Kitchens => Set<Kitchen>();
    public DbSet<KitchenOrder> KitchenOrders => Set<KitchenOrder>();
    public DbSet<OrderReadModel> OrderReadModels => Set<OrderReadModel>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<KitchenOrder>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.OrderId)
                .IsUnique(); 
        });
    }
}