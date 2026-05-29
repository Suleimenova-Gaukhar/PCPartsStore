using System.Collections.Generic;
using System.Linq;
using PCPartsStore.Models;

namespace PCPartsStore.Services;

public class CartService
{
    private readonly List<CartItem> _items = new();

    public IReadOnlyList<CartItem> Items => _items;

    public decimal Total => _items.Sum(i => i.Subtotal);

    public void Add(Component component)
    {
        var existing = _items.FirstOrDefault(i => i.Component.Id == component.Id);
        if (existing is not null)
            existing.Quantity++;
        else
            _items.Add(new CartItem { Component = component });
    }

    public void Remove(CartItem item)
    {
        _items.Remove(item);
    }

    public void RemoveByComponent(Component component)
    {
        var item = _items.FirstOrDefault(i => i.Component.Id == component.Id);
        if (item is not null)
            _items.Remove(item);
    }

    public void UpdateQuantity(CartItem item, int quantity)
    {
        if (quantity <= 0)
            _items.Remove(item);
        else
            item.Quantity = quantity;
    }

    public void Clear()
    {
        _items.Clear();
    }
}