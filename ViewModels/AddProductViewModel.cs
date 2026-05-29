using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PCPartsStore.Models;
using PCPartsStore.Services;

namespace PCPartsStore.ViewModels;

public partial class AddProductViewModel : ViewModelBase
{
    private readonly ProductService _productService;
    private readonly MainWindowViewModel _mainVm;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _manufacturer = string.Empty;

    [ObservableProperty]
    private decimal _price;

    [ObservableProperty]
    private int _stock;

    [ObservableProperty]
    private Category _selectedCategory;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    public List<Category> Categories { get; } = Enum.GetValues<Category>().ToList();

    public AddProductViewModel(ProductService productService, MainWindowViewModel mainVm)
    {
        _productService = productService;
        _mainVm = mainVm;
    }

    [RelayCommand]
    private void SaveProduct()
    {
        if (string.IsNullOrWhiteSpace(Name) ||
            string.IsNullOrWhiteSpace(Manufacturer) ||
            Price <= 0 || Stock < 0)
        {
            StatusMessage = "Please fill in all fields correctly.";
            return;
        }

        var component = new Component
        {
            Name = Name,
            Manufacturer = Manufacturer,
            Price = Price,
            Stock = Stock,
            Category = SelectedCategory
        };

        _productService.Add(component);
        _mainVm.NavigateToProducts();
    }

    [RelayCommand]
    private void GoBack()
    {
        _mainVm.NavigateToProducts();
    }
}