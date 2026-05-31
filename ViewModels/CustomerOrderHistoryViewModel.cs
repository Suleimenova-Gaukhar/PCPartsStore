using System.Collections.ObjectModel;
using Avalonia.Data.Converters;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PCPartsStore.Models;
using PCPartsStore.Services;

namespace PCPartsStore.ViewModels;

public partial class CustomerOrderHistoryViewModel : ViewModelBase
{
    private readonly OrderService _orderService;
    private readonly MainWindowViewModel _mainVm;

    [ObservableProperty]
    private ObservableCollection<Order> _orders = new();

    [ObservableProperty]
    private Order? _selectedOrder;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private bool _isSuccess;

    public static IValueConverter SelectedRowConverter { get; } =
        new FuncValueConverter<object, IBrush>(_ =>
            new SolidColorBrush(Colors.Transparent));

    public static IValueConverter SuccessColorConverter { get; } =
        new FuncValueConverter<bool, IBrush>(isSuccess =>
            isSuccess
                ? new SolidColorBrush(Color.Parse("#6BAA75"))
                : new SolidColorBrush(Color.Parse("#F9564F")));

    public CustomerOrderHistoryViewModel(OrderService orderService, MainWindowViewModel mainVm, bool showSuccess = false)
    {
        _orderService = orderService;
        _mainVm = mainVm;
        Orders = new ObservableCollection<Order>(_orderService.GetAll());

        if (showSuccess)
        {
            StatusMessage = "✓ Your order has been placed successfully!";
            IsSuccess = true;
        }
    }

    [RelayCommand]
    private void CancelOrder(Order? order)
    {
        if (order is null) return;
        var success = _orderService.CancelOrder(order.Id);
        if (success)
        {
            IsSuccess = false;
            StatusMessage = "Order cancelled successfully.";
            Orders = new ObservableCollection<Order>(_orderService.GetAll());
            SelectedOrder = null;
        }
        else
        {
            IsSuccess = false;
            StatusMessage = "This order cannot be cancelled.";
        }
    }
}