using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using PCPartsStore.Models;

namespace PCPartsStore.Services;

public class ProductService
{
    private readonly string _filePath = "Data/products.json";

    public List<Component> GetAll()
    {
        if (!File.Exists(_filePath))
            return new List<Component>();

        var json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<List<Component>>(json) ?? new List<Component>();
    }

    public void SaveAll(List<Component> components)
    {
        var json = JsonSerializer.Serialize(components, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }

    public void Add(Component component)
    {
        var all = GetAll();
        all.Add(component);
        SaveAll(all);
    }

    public void Update(Component updated)
    {
        var all = GetAll();
        var index = all.FindIndex(c => c.Id == updated.Id);
        if (index >= 0)
            all[index] = updated;
        SaveAll(all);
    }

    public void Delete(Guid id)
    {
        var all = GetAll();
        all.RemoveAll(c => c.Id == id);
        SaveAll(all);
    }
}