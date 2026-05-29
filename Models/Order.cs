using System;
using System.Collections.Generic;
using System.Linq;

namespace PCPartsStore.Models;

public enum OrderStatus
{
    Confirmed,
    OnTheWay,
    Delivered,
    Cancelled
}

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Client Client { get; set; } = new();
    public List<CartItem> Items { get; set; } = new();
    public DateTime PlacedAt { get; set; } = DateTime.Now;
    public OrderStatus Status { get; set; } = OrderStatus.Confirmed;

    public decimal Total => Items.Sum(i => i.Subtotal);
    public bool CanChangeStatus => Status != OrderStatus.Delivered;
    public bool CanCustomerCancel => Status == OrderStatus.Confirmed;
}