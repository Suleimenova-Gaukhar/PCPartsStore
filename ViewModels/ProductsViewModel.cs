using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PCPartsStore.Models;
using PCPartsStore.Services;

namespace PCPartsStore.ViewModels;

public partial class ProductsViewModel : ViewModelBase
{
    private readonly ProductService _productService;
    private readonly MainWindowViewModel _mainVm;

    [ObservableProperty]
    private ObservableCollection<Component> _components = new();

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private string _selectedCategory = "All";

    public List<string> Categories { get; } =
        new() { "All", "CPU", "GPU", "RAM", "Motherboard", "Storage", "PSU", "Case", "Cooling" };

    public ProductsViewModel(ProductService productService, MainWindowViewModel mainVm)
    {
        _productService = productService;
        _mainVm = mainVm;
        ApplyFilter();
    }

    partial void OnSearchTextChanged(string value) => ApplyFilter();
    partial void OnSelectedCategoryChanged(string value) => ApplyFilter();

    private void ApplyFilter()
    {
        var filtered = _productService.Search(SearchText, SelectedCategory);
        Components = new ObservableCollection<Component>(filtered);
    }

    [RelayCommand]
    private void AddProduct()
    {
        _mainVm.NavigateToAddProduct();
    }

    [RelayCommand]
    private void EditProduct(Component? component)
    {
        if (component is null) return;
        _mainVm.NavigateToEditProduct(component);
    }
}