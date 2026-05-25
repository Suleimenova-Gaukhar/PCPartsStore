using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PCPartsStore.Models;
using PCPartsStore.Services;

namespace PCPartsStore.ViewModels;

public partial class AdminViewModel : ViewModelBase
{
    private readonly ProductService _productService;

    [ObservableProperty]
    private ObservableCollection<Component> _components;

    [ObservableProperty]
    private Component? _selectedComponent;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _manufacturer = string.Empty;

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private decimal _price;

    [ObservableProperty]
    private int _stock;

    [ObservableProperty]
    private Category _selectedCategory;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    public List<Category> Categories { get; } = Enum.GetValues<Category>().ToList();

    public AdminViewModel(ProductService productService)
    {
        _productService = productService;
        _components = new ObservableCollection<Component>(_productService.GetAll());
    }

    partial void OnSelectedComponentChanged(Component? value)
    {
        if (value is null) return;
        Name = value.Name;
        Manufacturer = value.Manufacturer;
        Description = value.Description;
        Price = value.Price;
        Stock = value.Stock;
        SelectedCategory = value.Category;
    }

    [RelayCommand]
    private void AddComponent()
    {
        var component = new Component
        {
            Name = Name,
            Manufacturer = Manufacturer,
            Description = Description,
            Price = Price,
            Stock = Stock,
            Category = SelectedCategory
        };

        _productService.Add(component);
        Components.Add(component);
        StatusMessage = $"{component.Name} added successfully.";
        ClearForm();
    }

    [RelayCommand]
    private void UpdateComponent()
    {
        if (SelectedComponent is null) return;

        SelectedComponent.Name = Name;
        SelectedComponent.Manufacturer = Manufacturer;
        SelectedComponent.Description = Description;
        SelectedComponent.Price = Price;
        SelectedComponent.Stock = Stock;
        SelectedComponent.Category = SelectedCategory;

        _productService.Update(SelectedComponent);
        Components = new ObservableCollection<Component>(_productService.GetAll());
        StatusMessage = "Component updated successfully.";
    }

    [RelayCommand]
    private void DeleteComponent()
    {
        if (SelectedComponent is null) return;
        _productService.Delete(SelectedComponent.Id);
        Components.Remove(SelectedComponent);
        StatusMessage = "Component deleted.";
        ClearForm();
    }

    private void ClearForm()
    {
        Name = string.Empty;
        Manufacturer = string.Empty;
        Description = string.Empty;
        Price = 0;
        Stock = 0;
        SelectedComponent = null;
    }
}