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
                new Component { Name = "AMD Ryzen 7 5700X", Manufacturer = "AMD", Category = Category.CPU, Price = 799.00m, Stock = 10 },
                new Component { Name = "AMD Ryzen 7 9800X3D", Manufacturer = "AMD", Category = Category.CPU, Price = 2199.00m, Stock = 8 },
                new Component { Name = "AMD Ryzen 5 5500", Manufacturer = "AMD", Category = Category.CPU, Price = 449.00m, Stock = 15 },
                new Component { Name = "Intel Core i5-14600KF", Manufacturer = "Intel", Category = Category.CPU, Price = 1199.00m, Stock = 12 },
                new Component { Name = "Intel Core i5-12400F", Manufacturer = "Intel", Category = Category.CPU, Price = 609.00m, Stock = 10 },
                new Component { Name = "AMD Ryzen 5 9600X", Manufacturer = "AMD", Category = Category.CPU, Price = 899.00m, Stock = 7 },
                new Component { Name = "AMD Ryzen 9 7900", Manufacturer = "AMD", Category = Category.CPU, Price = 1599.00m, Stock = 4 },
                new Component { Name = "Intel Core i5-14600KF Box", Manufacturer = "Intel", Category = Category.CPU, Price = 1279.00m, Stock = 6 },

                new Component { Name = "Asus GeForce RTX 5060 Ti 16GB", Manufacturer = "Asus", Category = Category.GPU, Price = 3199.00m, Stock = 5 },
                new Component { Name = "Gigabyte GeForce RTX 5070 12GB", Manufacturer = "Gigabyte", Category = Category.GPU, Price = 3399.00m, Stock = 4 },
                new Component { Name = "Gigabyte GeForce RTX 5090 32GB", Manufacturer = "Gigabyte", Category = Category.GPU, Price = 23499.00m, Stock = 2 },
                new Component { Name = "AsRock Arc B580 12GB", Manufacturer = "AsRock", Category = Category.GPU, Price = 1599.00m, Stock = 8 },
                new Component { Name = "MSI GeForce RTX 5060 8GB", Manufacturer = "MSI", Category = Category.GPU, Price = 1749.00m, Stock = 6 },
                new Component { Name = "Gigabyte GeForce RTX 5060 8GB", Manufacturer = "Gigabyte", Category = Category.GPU, Price = 1799.00m, Stock = 0 },
                new Component { Name = "Asus GeForce RTX 5070 ROG Strix", Manufacturer = "Asus", Category = Category.GPU, Price = 4349.00m, Stock = 3 },
                new Component { Name = "Asus GeForce RTX 5070 Dual", Manufacturer = "Asus", Category = Category.GPU, Price = 3199.00m, Stock = 4 },

                new Component { Name = "Kit RAM Corsair DDR4 32GB 3200MHz", Manufacturer = "Corsair", Category = Category.RAM, Price = 1299.00m, Stock = 20 },
                new Component { Name = "Kit RAM Kingston DDR5 64GB 6000MHz", Manufacturer = "Kingston", Category = Category.RAM, Price = 4099.00m, Stock = 8 },
                new Component { Name = "Kit RAM Kingston DDR4 32GB 3200MHz FURY Beast", Manufacturer = "Kingston", Category = Category.RAM, Price = 1099.00m, Stock = 15 },
                new Component { Name = "RAM Adata DDR4 16GB 3200MHz", Manufacturer = "Adata", Category = Category.RAM, Price = 599.00m, Stock = 25 },
                new Component { Name = "Kit RAM Kingston DDR4 32GB FURY Beast", Manufacturer = "Kingston", Category = Category.RAM, Price = 1199.00m, Stock = 12 },
                new Component { Name = "RAM G.Skill DDR5 16GB 6000MHz Flare X5", Manufacturer = "G.Skill", Category = Category.RAM, Price = 1399.00m, Stock = 3 },
                new Component { Name = "Kit RAM Kingston DDR5 64GB FURY Beast", Manufacturer = "Kingston", Category = Category.RAM, Price = 3999.00m, Stock = 5 },
                new Component { Name = "Kit RAM Adata DDR5 32GB 6000MHz XPG", Manufacturer = "Adata", Category = Category.RAM, Price = 2499.00m, Stock = 7 },

                new Component { Name = "Gigabyte B650 Eagle AX AM5", Manufacturer = "Gigabyte", Category = Category.Motherboard, Price = 654.00m, Stock = 9 },
                new Component { Name = "MSI A520M-A PRO AM4", Manufacturer = "MSI", Category = Category.Motherboard, Price = 249.00m, Stock = 14 },
                new Component { Name = "Gigabyte B650M D3HP AM5", Manufacturer = "Gigabyte", Category = Category.Motherboard, Price = 539.00m, Stock = 10 },
                new Component { Name = "Gigabyte B550 Eagle WiFi AM4", Manufacturer = "Gigabyte", Category = Category.Motherboard, Price = 579.00m, Stock = 8 },
                new Component { Name = "MSI B550 ATX WiFi5 Gaming AM4", Manufacturer = "MSI", Category = Category.Motherboard, Price = 529.00m, Stock = 6 },
                new Component { Name = "Gigabyte X870 Gaming WiFi AM5", Manufacturer = "Gigabyte", Category = Category.Motherboard, Price = 969.00m, Stock = 4 },
                new Component { Name = "Gigabyte B650M D3HP AX AM5", Manufacturer = "Gigabyte", Category = Category.Motherboard, Price = 619.00m, Stock = 7 },
                new Component { Name = "MSI H610M PRO DDR4", Manufacturer = "MSI", Category = Category.Motherboard, Price = 279.00m, Stock = 11 },

                new Component { Name = "SSD 1TB Adata M.2 NVMe XPG S20G", Manufacturer = "Adata", Category = Category.Storage, Price = 749.00m, Stock = 18 },
                new Component { Name = "SSD 500GB Adata M.2 NVMe Legend 860", Manufacturer = "Adata", Category = Category.Storage, Price = 439.00m, Stock = 22 },
                new Component { Name = "SSD 512GB Apacer M.2 NVMe AS2280P4", Manufacturer = "Apacer", Category = Category.Storage, Price = 419.00m, Stock = 16 },
                new Component { Name = "SSD 240GB Kingston 2.5 A400", Manufacturer = "Kingston", Category = Category.Storage, Price = 344.00m, Stock = 30 },
                new Component { Name = "SSD 2TB Corsair M.2 NVMe MP600 PRO", Manufacturer = "Corsair", Category = Category.Storage, Price = 1299.00m, Stock = 8 },
                new Component { Name = "SSD 1TB Verbatim 2.5 VI550 S3", Manufacturer = "Verbatim", Category = Category.Storage, Price = 599.00m, Stock = 12 },
                new Component { Name = "SSD 2TB Samsung M.2 NVMe 9100 PRO", Manufacturer = "Samsung", Category = Category.Storage, Price = 1999.00m, Stock = 5 },
                new Component { Name = "SSD 1TB WD M.2 NVMe SN850X", Manufacturer = "Western Digital", Category = Category.Storage, Price = 999.00m, Stock = 9 },

                new Component { Name = "UPS 2200VA APC Smart-UPS 9AC", Manufacturer = "APC", Category = Category.PSU, Price = 15333.00m, Stock = 2 },
                new Component { Name = "UPS 2000VA 1600W Njoy ECHO PRO", Manufacturer = "Njoy", Category = Category.PSU, Price = 1250.00m, Stock = 6 },
                new Component { Name = "UPS 2000VA 1200W Njoy UPLI-LI200CO", Manufacturer = "Njoy", Category = Category.PSU, Price = 895.00m, Stock = 8 },
                new Component { Name = "UPS 2000VA 1200W Njoy Horus Plus", Manufacturer = "Njoy", Category = Category.PSU, Price = 482.00m, Stock = 10 },
                new Component { Name = "UPS 3000VA 2700W APC Smart-UPS", Manufacturer = "APC", Category = Category.PSU, Price = 8974.00m, Stock = 3 },
                new Component { Name = "UPS 800VA 480W Njoy Horus 800", Manufacturer = "Njoy", Category = Category.PSU, Price = 349.00m, Stock = 15 },
                new Component { Name = "UPS 1600VA 900W APC", Manufacturer = "APC", Category = Category.PSU, Price = 780.00m, Stock = 7 },
                new Component { Name = "UPS 1500VA 1050W Kemot", Manufacturer = "Kemot", Category = Category.PSU, Price = 669.00m, Stock = 5 },

                new Component { Name = "Carcasa Segotep Endura 240S Mini Tower", Manufacturer = "Segotep", Category = Category.Case, Price = 249.00m, Stock = 12 },
                new Component { Name = "Carcasa Gaming PCCOOLER MA100 MESH", Manufacturer = "PCCOOLER", Category = Category.Case, Price = 179.00m, Stock = 18 },
                new Component { Name = "Carcasa Segotep Alpha ATX Mini Tower", Manufacturer = "Segotep", Category = Category.Case, Price = 229.00m, Stock = 10 },
                new Component { Name = "Carcasa ProGaming Kian ARGB Middle Tower", Manufacturer = "ProGaming", Category = Category.Case, Price = 379.00m, Stock = 7 },
                new Component { Name = "Carcasa Inaza Cube Tower M-ATX ARGB", Manufacturer = "Inaza", Category = Category.Case, Price = 229.00m, Stock = 9 },
                new Component { Name = "Carcasa Segotep V5 Black", Manufacturer = "Segotep", Category = Category.Case, Price = 119.00m, Stock = 20 },
                new Component { Name = "Carcasa MSI MAG Forge M100A", Manufacturer = "MSI", Category = Category.Case, Price = 196.00m, Stock = 14 },
                new Component { Name = "Carcasa PcCOOLER C3D510 ARGB", Manufacturer = "PcCOOLER", Category = Category.Case, Price = 219.00m, Stock = 11 },

                new Component { Name = "Ventilator ARCTIC AC P12 Pro PWM A-RGB", Manufacturer = "Arctic", Category = Category.Cooling, Price = 54.00m, Stock = 35 },
                new Component { Name = "Kit 5x Ventilator ARCTIC P12 120mm", Manufacturer = "Arctic", Category = Category.Cooling, Price = 79.00m, Stock = 22 },
                new Component { Name = "Kit 3 Ventilatoare AQIRYS Libra 120mm ARGB", Manufacturer = "AQIRYS", Category = Category.Cooling, Price = 179.00m, Stock = 16 },
                new Component { Name = "Set 3 Ventilatoare Corsair RS120 ARGB", Manufacturer = "Corsair", Category = Category.Cooling, Price = 179.00m, Stock = 14 },
                new Component { Name = "Ventilator ID-Cooling AS-120 ARGB", Manufacturer = "ID-Cooling", Category = Category.Cooling, Price = 39.00m, Stock = 28 },
                new Component { Name = "Kit Ventilatoare 1stPlayer COMBO CC ARGB", Manufacturer = "1stPlayer", Category = Category.Cooling, Price = 89.00m, Stock = 19 },
                new Component { Name = "Kit Ventilatoare Gamemax FN12A-S3I RGB", Manufacturer = "Gamemax", Category = Category.Cooling, Price = 127.00m, Stock = 4 },
                new Component { Name = "Ventilator Noctua NF-F12 PWM 120mm", Manufacturer = "Noctua", Category = Category.Cooling, Price = 112.00m, Stock = 8 },
            };

            Components.AddRange(components);
            SaveChanges();
        }
}