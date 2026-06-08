using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using PCPartsStore.Models;
using PCPartsStore.Services;

namespace PCPartsStore.ViewModels;

public partial class OrderRow : ObservableObject
{
    private readonly OrderService _orderService;
    private readonly AllOrdersViewModel _parentVm;

    public Order Order { get; }

    [ObservableProperty]
    private OrderStatus _selectedStatus;

    public bool CanChangeStatus => Order.CanChangeStatus;

    public List<OrderStatus> Statuses { get; } = new List<OrderStatus>
    {
        OrderStatus.Confirmed,
        OrderStatus.OnTheWay,
        OrderStatus.Delivered,
        OrderStatus.Cancelled
    };

    public OrderRow(Order order, OrderService orderService, AllOrdersViewModel parentVm)
    {
        Order = order;
        _orderService = orderService;
        _parentVm = parentVm;
        _selectedStatus = order.Status;
    }

    partial void OnSelectedStatusChanged(OrderStatus value)
    {
        if (!Order.CanChangeStatus) return;
        if (value == Order.Status) return;

        var success = _orderService.UpdateOrderStatus(Order.Id, value);
        if (success)
        {
            Order.Status = value;
            OnPropertyChanged(nameof(CanChangeStatus));
            _parentVm.RefreshOrders();
        }
        else
        {
            SelectedStatus = Order.Status;
        }
    }
}

public partial class AllOrdersViewModel : ViewModelBase
{
    private readonly OrderService _orderService;

    [ObservableProperty]
    private ObservableCollection<OrderRow> _orders = new();

    [ObservableProperty]
    private OrderRow? _selectedOrderRow;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    public AllOrdersViewModel(OrderService orderService)
    {
        _orderService = orderService;
        RefreshOrders();
    }

    public void RefreshOrders()
    {
        var orders = _orderService.GetAll();
        Orders = new ObservableCollection<OrderRow>(
            orders.Select(o => new OrderRow(o, _orderService, this))
        );

        if (SelectedOrderRow is not null)
        {
            var updated = Orders.FirstOrDefault(r => r.Order.Id == SelectedOrderRow.Order.Id);
            SelectedOrderRow = updated;
        }
    }
}