using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PCPartsStore.Data;
using PCPartsStore.Models;

namespace PCPartsStore.Services;

public class ProductService
{
    public List<Component> GetAll()
    {
        using var context = new AppDbContext();
        return context.Components.ToList();
    }

    public List<Component> Search(string searchText, string category)
    {
        using var context = new AppDbContext();
        var query = context.Components.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchText))
            query = query.Where(c =>
                c.Name.Contains(searchText) ||
                c.Manufacturer.Contains(searchText));

        if (category != "All")
        {
            if (Enum.TryParse<Category>(category, out var categoryEnum))
                query = query.Where(c => c.Category == categoryEnum);
        }

        return query.ToList();
    }

    public void Add(Component component)
    {
        using var context = new AppDbContext();
        context.Components.Add(component);
        context.SaveChanges();
    }

    public void Update(Component component)
    {
        using var context = new AppDbContext();
        context.Components.Update(component);
        context.SaveChanges();
    }

    public void Delete(Guid id)
    {
        using var context = new AppDbContext();
        var component = context.Components.Find(id);
        if (component is not null)
        {
            context.Components.Remove(component);
            context.SaveChanges();
        }
    }

    public void SaveAll(List<Component> components)
    {
        using var context = new AppDbContext();
        context.Components.UpdateRange(components);
        context.SaveChanges();
    }
}