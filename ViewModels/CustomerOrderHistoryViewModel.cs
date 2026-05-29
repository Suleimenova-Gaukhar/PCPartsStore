using System.Collections.ObjectModel;
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

    public CustomerOrderHistoryViewModel(OrderService orderService, MainWindowViewModel mainVm)
    {
        _orderService = orderService;
        _mainVm = mainVm;
        Orders = new ObservableCollection<Order>(_orderService.GetAll());
    }

    [RelayCommand]
    private void CancelOrder()
    {
        if (SelectedOrder is null) return;
        var success = _orderService.CancelOrder(SelectedOrder.Id);
        if (success)
        {
            StatusMessage = "Order cancelled successfully.";
            Orders = new ObservableCollection<Order>(_orderService.GetAll());
        }
        else
        {
            StatusMessage = "This order cannot be cancelled.";
        }
    }
}