using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PCPartsStore.Models;
using PCPartsStore.Services;

namespace PCPartsStore.ViewModels;

public partial class AllOrdersViewModel : ViewModelBase
{
    private readonly OrderService _orderService;

    [ObservableProperty]
    private ObservableCollection<Order> _orders = new();

    [ObservableProperty]
    private Order? _selectedOrder;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    public List<OrderStatus> Statuses { get; } = Enum.GetValues<OrderStatus>().ToList();

    public AllOrdersViewModel(OrderService orderService)
    {
        _orderService = orderService;
        Orders = new ObservableCollection<Order>(_orderService.GetAll());
    }

    [RelayCommand]
    private void UpdateStatus(Order? order)
    {
        if (order is null) return;
        var success = _orderService.UpdateOrderStatus(order.Id, order.Status);
        if (success)
        {
            StatusMessage = "Order status updated.";
            Orders = new ObservableCollection<Order>(_orderService.GetAll());
        }
        else
        {
            StatusMessage = "Cannot update this order.";
        }
    }
}