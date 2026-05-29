using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PCPartsStore.Models;
using PCPartsStore.Services;

namespace PCPartsStore.ViewModels;

public partial class CheckoutViewModel : ViewModelBase
{
    private readonly CartService _cartService;
    private readonly OrderService _orderService;
    private readonly MainWindowViewModel _mainVm;

    [ObservableProperty]
    private string _customerName = string.Empty;

    [ObservableProperty]
    private string _customerEmail = string.Empty;

    [ObservableProperty]
    private string _customerPhone = string.Empty;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private decimal _total;

    public CheckoutViewModel(CartService cartService, OrderService orderService, MainWindowViewModel mainVm)
    {
        _cartService = cartService;
        _orderService = orderService;
        _mainVm = mainVm;
        _total = _cartService.Total;
    }

    [RelayCommand]
    private void PlaceOrder()
    {
        if (string.IsNullOrWhiteSpace(CustomerName) ||
            string.IsNullOrWhiteSpace(CustomerEmail) ||
            string.IsNullOrWhiteSpace(CustomerPhone))
        {
            StatusMessage = "Please fill in all fields.";
            return;
        }

        var order = new Order
        {
            Client = new Client
            {
                Name = CustomerName,
                Email = CustomerEmail,
                Phone = CustomerPhone
            },
            Items = _cartService.Items.ToList()
        };

        var success = _orderService.PlaceOrder(order);

        if (success)
        {
            _cartService.Clear();
            StatusMessage = "Order placed successfully!";
            _mainVm.NavigateToMyOrders();
        }
        else
        {
            StatusMessage = "Some items are out of stock. Please review your cart.";
        }
    }

    [RelayCommand]
    private void GoBack()
    {
        _mainVm.NavigateToCart();
    }
}