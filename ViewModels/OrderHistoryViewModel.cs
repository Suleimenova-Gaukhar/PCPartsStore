using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using PCPartsStore.Models;
using PCPartsStore.Services;

namespace PCPartsStore.ViewModels;

public partial class OrderHistoryViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<Order> _orders;

    [ObservableProperty]
    private Order? _selectedOrder;

    public OrderHistoryViewModel(OrderService orderService)
    {
        _orders = new ObservableCollection<Order>(orderService.GetAll());
    }
}