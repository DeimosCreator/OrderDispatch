using KitchenService.Models.Entities;
using Microsoft.EntityFrameworkCore;
using OrderDispatch.Contracts.Abstractions;

namespace KitchenService.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Kitchen> Kitchens => Set<Kitchen>();
    public DbSet<KitchenOrder> KitchenOrders => Set<KitchenOrder>();
    public DbSet<KitchenStaff> KitchenStaffs => Set<KitchenStaff>();
    public DbSet<KitchenOrderItem> KitchenOrderItems => Set<KitchenOrderItem>();
    
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
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<IAuditable>())
        {
            if (entry.State is EntityState.Added or EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}