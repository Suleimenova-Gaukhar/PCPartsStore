using System;
namespace PCPartsStore.Models;

public class Component
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;
    public Category Category { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }

    public string Status => Stock switch
    {
        0 => "Sold out",
        <= 4 => "Low stock",
        _ => "Active"
    };
}