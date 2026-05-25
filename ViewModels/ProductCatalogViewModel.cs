using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PCPartsStore.Models;
using PCPartsStore.Services;

namespace PCPartsStore.ViewModels;

public partial class ProductCatalogViewModel : ViewModelBase
{
    private readonly ProductService _productService;
    private readonly CartService _cartService;
    private readonly MainWindowViewModel _mainVm;
    private List<Component> _allComponents = new();

    [ObservableProperty]
    private ObservableCollection<Component> _components = new();

    [ObservableProperty]
    private Component? _selectedComponent;

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private string _selectedCategory = "All";

    public List<string> Categories { get; } =
        new() { "All", "CPU", "GPU", "RAM", "Motherboard", "Storage", "PSU", "Case", "Cooling" };

    public ProductCatalogViewModel(ProductService productService, CartService cartService, MainWindowViewModel mainVm)
    {
        _productService = productService;
        _cartService = cartService;
        _mainVm = mainVm;
        LoadComponents();
    }

    private void LoadComponents()
    {
        _allComponents = _productService.GetAll();
        ApplyFilter();
    }

    partial void OnSearchTextChanged(string value) => ApplyFilter();
    partial void OnSelectedCategoryChanged(string value) => ApplyFilter();

    private void ApplyFilter()
    {
        var filtered = _allComponents.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(SearchText))
            filtered = filtered.Where(c =>
                c.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                c.Manufacturer.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

        if (SelectedCategory != "All")
            filtered = filtered.Where(c => c.Category.ToString() == SelectedCategory);

        Components = new ObservableCollection<Component>(filtered);
    }

    [RelayCommand]
    private void AddToCart()
    {
        if (SelectedComponent is null) return;
        _cartService.Add(SelectedComponent);
    }

    [RelayCommand]
    private void GoToCart()
    {
        _mainVm.NavigateToCart();
    }
}