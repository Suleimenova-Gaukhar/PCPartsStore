using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using PCPartsStore.Models;

namespace PCPartsStore.Services;

public class OrderService
{
    private readonly string _filePath = "Data/orders.json";
    private readonly ProductService _productService;

    public OrderService(ProductService productService)
    {
        _productService = productService;
    }

    public List<Order> GetAll()
    {
        if (!File.Exists(_filePath))
            return new List<Order>();

        var json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<List<Order>>(json) ?? new List<Order>();
    }

    public bool PlaceOrder(Order order)
    {
        var components = _productService.GetAll();

        foreach (var item in order.Items)
        {
            var component = components.FirstOrDefault(c => c.Id == item.Component.Id);
            if (component is null || component.Stock < item.Quantity)
                return false;

            component.Stock -= item.Quantity;
        }

        _productService.SaveAll(components);

        var orders = GetAll();
        orders.Add(order);

        var json = JsonSerializer.Serialize(orders, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);

        return true;
    }
}