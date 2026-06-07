using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PCPartsStore.Models;
using PCPartsStore.Services;

namespace PCPartsStore.ViewModels;

public partial class EditProductViewModel : ViewModelBase
{
    private readonly ProductService _productService;
    private readonly MainWindowViewModel _mainVm;
    private readonly Component _originalComponent;

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

    [ObservableProperty]
    private bool _showDeleteConfirmation;

    public List<Category> Categories { get; } = Enum.GetValues<Category>().ToList();

    public EditProductViewModel(ProductService productService, Component component, MainWindowViewModel mainVm)
    {
        _productService = productService;
        _mainVm = mainVm;
        _originalComponent = component;

        Name = component.Name;
        Manufacturer = component.Manufacturer;
        Price = component.Price;
        Stock = component.Stock;
        SelectedCategory = component.Category;
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

        _originalComponent.Name = Name;
        _originalComponent.Manufacturer = Manufacturer;
        _originalComponent.Price = Price;
        _originalComponent.Stock = Stock;
        _originalComponent.Category = SelectedCategory;

        _productService.Update(_originalComponent);
        _mainVm.NavigateToProducts();
    }

    [RelayCommand]
    private void RequestDelete()
    {
        ShowDeleteConfirmation = true;
        StatusMessage = "Are you sure you want to delete this product?";
    }

    [RelayCommand]
    private void ConfirmDelete()
    {
        _productService.Delete(_originalComponent.Id);
        _mainVm.NavigateToProducts();
    }

    [RelayCommand]
    private void CancelDelete()
    {
        ShowDeleteConfirmation = false;
        StatusMessage = string.Empty;
    }

    [RelayCommand]
    private void GoBack()
    {
        _mainVm.NavigateToProducts();
    }
}