using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PCPartsStore.Data;
using PCPartsStore.Models;

namespace PCPartsStore.Services;

public class OrderService
{
    private readonly ProductService _productService;

    public OrderService(ProductService productService)
    {
        _productService = productService;
    }

    public List<Order> GetAll()
    {
        using var context = new AppDbContext();
        return context.Orders
            .AsNoTracking()
            .Include(o => o.Client)
            .Include(o => o.Items)
            .ThenInclude(i => i.Component)
            .ToList();
    }

    public bool PlaceOrder(Order order)
    {
        using var context = new AppDbContext();

        foreach (var item in order.Items)
        {
            var component = context.Components
                .FirstOrDefault(c => c.Id == item.Component.Id);

            if (component is null || component.Stock < item.Quantity)
                return false;

            component.Stock -= item.Quantity;
        }

        var newOrder = new Order
        {
            Id = order.Id,
            PlacedAt = order.PlacedAt,
            Status = order.Status,
            Client = new Client
            {
                Name = order.Client.Name,
                Email = order.Client.Email,
                Phone = order.Client.Phone
            },
            Items = order.Items.Select(i => new CartItem
            {
                Quantity = i.Quantity,
                Component = context.Components
                    .First(c => c.Id == i.Component.Id)
            }).ToList()
        };

        context.Orders.Add(newOrder);
        context.SaveChanges();
        return true;
    }

    public bool UpdateOrderStatus(Guid orderId, OrderStatus newStatus)
        {
            using var context = new AppDbContext();
            var order = context.Orders
                .FirstOrDefault(o => o.Id == orderId);
            if (order is null || !order.CanChangeStatus)
                return false;
            order.Status = newStatus;
            context.SaveChanges();
            return true;
        }

    public bool CancelOrder(Guid orderId)
    {
        using var context = new AppDbContext();
        var order = context.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Component)
            .FirstOrDefault(o => o.Id == orderId);

        if (order is null || !order.CanCustomerCancel)
            return false;

        foreach (var item in order.Items)
        {
            var component = context.Components.Find(item.Component.Id);
            if (component is not null)
                component.Stock += item.Quantity;
        }

        order.Status = OrderStatus.Cancelled;
        context.SaveChanges();
        return true;
    }

    public Dictionary<string, decimal> GetRevenueByCategory()
    {
        using var context = new AppDbContext();
        return context.CartItems
            .AsNoTracking()
            .Include(i => i.Component)
            .GroupBy(i => i.Component.Category.ToString())
            .ToDictionary(
                g => g.Key,
                g => g.Sum(i => i.Component.Price * i.Quantity)
            );
    }

    public decimal GetTotalSalesToday()
    {
        using var context = new AppDbContext();
        return context.Orders
            .AsNoTracking()
            .Include(o => o.Items)
            .ThenInclude(i => i.Component)
            .Where(o => o.PlacedAt.Date == DateTime.Today &&
                        o.Status != OrderStatus.Cancelled)
            .SelectMany(o => o.Items)
            .Sum(i => i.Component.Price * i.Quantity);
    }

    public decimal GetTotalSalesThisWeek()
    {
        using var context = new AppDbContext();
        var weekAgo = DateTime.Today.AddDays(-7);
        return context.Orders
            .AsNoTracking()
            .Include(o => o.Items)
            .ThenInclude(i => i.Component)
            .Where(o => o.PlacedAt >= weekAgo &&
                        o.Status != OrderStatus.Cancelled)
            .SelectMany(o => o.Items)
            .Sum(i => i.Component.Price * i.Quantity);
    }

    public decimal GetTotalSalesThisMonth()
    {
        using var context = new AppDbContext();
        return context.Orders
            .AsNoTracking()
            .Include(o => o.Items)
            .ThenInclude(i => i.Component)
            .Where(o => o.PlacedAt.Month == DateTime.Today.Month &&
                        o.PlacedAt.Year == DateTime.Today.Year &&
                        o.Status != OrderStatus.Cancelled)
            .SelectMany(o => o.Items)
            .Sum(i => i.Component.Price * i.Quantity);
    }

    public decimal GetTotalSalesThisYear()
    {
        using var context = new AppDbContext();
        return context.Orders
            .AsNoTracking()
            .Include(o => o.Items)
            .ThenInclude(i => i.Component)
            .Where(o => o.PlacedAt.Year == DateTime.Today.Year &&
                        o.Status != OrderStatus.Cancelled)
            .SelectMany(o => o.Items)
            .Sum(i => i.Component.Price * i.Quantity);
    }
}