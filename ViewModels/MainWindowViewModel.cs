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

    [ObservableProperty]
    private bool _isAdmin;

    [ObservableProperty]
    private bool _isCustomer;

    [ObservableProperty]
    private bool _isSidebarVisible;

    public MainWindowViewModel()
    {
        _productService = new ProductService();
        _cartService = new CartService();
        _orderService = new OrderService(_productService);
        _currentView = new RoleSelectionViewModel(this);
        _isSidebarVisible = false;
    }

    public void SelectCustomer()
    {
        IsAdmin = false;
        IsCustomer = true;
        IsSidebarVisible = true;
        NavigateToCatalog();
    }

    public void SelectAdmin()
    {
        IsAdmin = true;
        IsCustomer = false;
        IsSidebarVisible = true;
        NavigateToProducts();
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
    public void NavigateToMyOrders()
    {
        CurrentView = new CustomerOrderHistoryViewModel(_orderService, this);
    }

    public void NavigateToMyOrdersWithSuccess()
    {
        CurrentView = new CustomerOrderHistoryViewModel(_orderService, this, showSuccess: true);
    }

    [RelayCommand]
    public void NavigateToProducts()
    {
        CurrentView = new ProductsViewModel(_productService, this);
    }

    [RelayCommand]
    public void NavigateToAllOrders()
    {
        CurrentView = new AllOrdersViewModel(_orderService);
    }

    [RelayCommand]
    public void NavigateToStatistics()
    {
        CurrentView = new StatisticsViewModel(_orderService);
    }

    public void NavigateToCheckout()
    {
        CurrentView = new CheckoutViewModel(_cartService, _orderService, this);
    }

    public void NavigateToAddProduct()
    {
        CurrentView = new AddProductViewModel(_productService, this);
    }

    public void NavigateToEditProduct(Models.Component component)
    {
        CurrentView = new EditProductViewModel(_productService, component, this);
    }

    [RelayCommand]
    public void SwitchAccount()
    {
        IsAdmin = false;
        IsCustomer = false;
        IsSidebarVisible = false;
        CurrentView = new RoleSelectionViewModel(this);
    }
}