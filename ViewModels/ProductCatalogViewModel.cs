using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Data.Converters;
using Avalonia.Media;
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

    [ObservableProperty]
    private ObservableCollection<ComponentRow> _components = new();

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private string _selectedCategory = "All";

    public List<string> Categories { get; } =
        new() { "All", "CPU", "GPU", "RAM", "Motherboard", "Storage", "PSU", "Case", "Cooling" };

    // Converters
    public static IValueConverter StatusToColorConverter { get; } =
        new FuncValueConverter<string, IBrush>(status => status switch
        {
            "Active" => new SolidColorBrush(Color.Parse("#6baa75")),
            "Low stock" => new SolidColorBrush(Color.Parse("#F9564F")),
            "Sold out" => new SolidColorBrush(Color.Parse("#888888")),
            _ => new SolidColorBrush(Color.Parse("#888888"))
        });

    public static IValueConverter StockToForegroundConverter { get; } =
        new FuncValueConverter<int, IBrush>(stock =>
            stock == 0
                ? new SolidColorBrush(Color.Parse("#AAAAAA"))
                : new SolidColorBrush(Color.Parse("#222222")));

    public static IValueConverter StockToRowBackgroundConverter { get; } =
        new FuncValueConverter<int, IBrush>(stock =>
            stock == 0
                ? new SolidColorBrush(Color.Parse("#F9F9F9"))
                : new SolidColorBrush(Colors.White));

    public static IValueConverter StockToBoolConverter { get; } =
        new FuncValueConverter<int, bool>(stock => stock > 0);

    public ProductCatalogViewModel(ProductService productService, CartService cartService, MainWindowViewModel mainVm)
    {
        _productService = productService;
        _cartService = cartService;
        _mainVm = mainVm;
        ApplyFilter();
    }

    partial void OnSearchTextChanged(string value) => ApplyFilter();
    partial void OnSelectedCategoryChanged(string value) => ApplyFilter();

    private void ApplyFilter()
    {
        var filtered = _productService.Search(SearchText, SelectedCategory);
        Components = new ObservableCollection<ComponentRow>(
            filtered.Select(c => new ComponentRow(c, IsInCart(c)))
        );
    }

    public bool IsInCart(Component component)
    {
        return _cartService.Items.Any(i => i.Component.Id == component.Id);
    }

    [RelayCommand]
    private void ToggleCart(ComponentRow? row)
    {
        if (row is null) return;
        if (row.Component.Stock == 0) return;

        if (row.IsInCart)
            _cartService.RemoveByComponent(row.Component);
        else
            _cartService.Add(row.Component);

        ApplyFilter();
    }

    [RelayCommand]
    private void GoToCart()
    {
        _mainVm.NavigateToCart();
    }
}

public class ComponentRow
{
    public Component Component { get; }
    public bool IsInCart { get; }

    public ComponentRow(Component component, bool isInCart)
    {
        Component = component;
        IsInCart = isInCart;
    }

    public string ButtonText => Component.Stock == 0 ? "Not available" :
                                IsInCart ? "Added to cart" : "Add to cart";

    public string ButtonColor => Component.Stock == 0 ? "#CCCCCC" :
                                 IsInCart ? "#4CAF50" : "#9984D4";

    public bool ButtonEnabled => Component.Stock > 0;
}