using System;
using System.Collections.Generic;
using System.Linq;

namespace PCPartsStore.Models;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Client Client { get; set; } = new();
    public List<CartItem> Items { get; set; } = new();
    public DateTime PlacedAt { get; set; } = DateTime.Now;
    public decimal Total 
    {
        get
        {
            return Items.Sum(i => i.Subtotal);
        }
    }
}