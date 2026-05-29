using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PCPartsStore.Models;

namespace PCPartsStore.Data;

public class AppDbContext : DbContext
{
    public DbSet<Component> Components { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<CartItem> CartItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=Data/pcparts.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Component>()
            .Ignore(c => c.Status);

        modelBuilder.Entity<CartItem>()
            .Ignore(c => c.Subtotal);

        modelBuilder.Entity<Order>()
            .Ignore(o => o.Total)
            .Ignore(o => o.CanChangeStatus)
            .Ignore(o => o.CanCustomerCancel);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Client)
            .WithMany()
            .HasForeignKey("ClientId");

        modelBuilder.Entity<Order>()
            .HasMany(o => o.Items)
            .WithOne()
            .HasForeignKey("OrderId");

        modelBuilder.Entity<CartItem>()
            .HasOne(c => c.Component)
            .WithMany()
            .HasForeignKey("ComponentId");
    }

    public void SeedData()
    {
        if (Components.Any()) return;

        var components = new List<Component>
        {
            new Component
            {
                Name = "Ryzen 5 5600X",
                Manufacturer = "AMD",
                Category = Category.CPU,
                Price = 180.00m,
                Stock = 15
            },
            new Component
            {
                Name = "Core i5-13600K",
                Manufacturer = "Intel",
                Category = Category.CPU,
                Price = 220.00m,
                Stock = 10
            },
            new Component
            {
                Name = "Ryzen 9 7900X",
                Manufacturer = "AMD",
                Category = Category.CPU,
                Price = 380.00m,
                Stock = 3
            },
            new Component
            {
                Name = "RTX 4060",
                Manufacturer = "Nvidia",
                Category = Category.GPU,
                Price = 299.99m,
                Stock = 8
            },
            new Component
            {
                Name = "RTX 4090",
                Manufacturer = "Nvidia",
                Category = Category.GPU,
                Price = 1599.99m,
                Stock = 2
            },
            new Component
            {
                Name = "RX 7600",
                Manufacturer = "AMD",
                Category = Category.GPU,
                Price = 249.99m,
                Stock = 0
            },
            new Component
            {
                Name = "Corsair 16GB DDR4",
                Manufacturer = "Corsair",
                Category = Category.RAM,
                Price = 45.00m,
                Stock = 30
            },
            new Component
            {
                Name = "G.Skill 32GB DDR5",
                Manufacturer = "G.Skill",
                Category = Category.RAM,
                Price = 89.99m,
                Stock = 4
            },
            new Component
            {
                Name = "Samsung 970 EVO 1TB",
                Manufacturer = "Samsung",
                Category = Category.Storage,
                Price = 89.99m,
                Stock = 20
            },
            new Component
            {
                Name = "WD Black 2TB",
                Manufacturer = "Western Digital",
                Category = Category.Storage,
                Price = 65.00m,
                Stock = 12
            },
            new Component
            {
                Name = "ROG Strix B550-F",
                Manufacturer = "ASUS",
                Category = Category.Motherboard,
                Price = 180.00m,
                Stock = 7
            },
            new Component
            {
                Name = "MSI MAG B660",
                Manufacturer = "MSI",
                Category = Category.Motherboard,
                Price = 140.00m,
                Stock = 0
            },
            new Component
            {
                Name = "Corsair RM850x",
                Manufacturer = "Corsair",
                Category = Category.PSU,
                Price = 120.00m,
                Stock = 9
            },
            new Component
            {
                Name = "NZXT H510",
                Manufacturer = "NZXT",
                Category = Category.Case,
                Price = 69.99m,
                Stock = 6
            },
            new Component
            {
                Name = "Noctua NH-D15",
                Manufacturer = "Noctua",
                Category = Category.Cooling,
                Price = 89.99m,
                Stock = 3
            }
        };

        Components.AddRange(components);
        SaveChanges();
    }
}