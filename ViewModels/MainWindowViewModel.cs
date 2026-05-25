using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PCPartsStore.Services;

namespace PCPartsStore.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly ProductService _productService;
    private readonly CartService _cartService;
    private readonly OrderService _orderService;

    [ObservableProperty]
    private ViewModelBase _currentView;

    public MainWindowViewModel()
    {
        _productService = new ProductService();
        _cartService = new CartService();
        _orderService = new OrderService(_productService);

        _currentView = new ProductCatalogViewModel(_productService, _cartService, this);
    }

    [RelayCommand]
    public void NavigateToCatalog()
    {
        CurrentView = new ProductCatalogViewModel(_productService, _cartService, this);
    }

    [RelayCommand]
    public void NavigateToCart()
    {
        CurrentView = new CartViewModel(_cartService, this);
    }

    [RelayCommand]
    public void NavigateToOrderHistory()
    {
        CurrentView = new OrderHistoryViewModel(_orderService);
    }

    [RelayCommand]
    public void NavigateToAdmin()
    {
        CurrentView = new AdminViewModel(_productService);
    }

    public void NavigateToCheckout()
    {
        CurrentView = new CheckoutViewModel(_cartService, _orderService, this);
    }
}