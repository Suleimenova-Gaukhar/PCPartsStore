using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PCPartsStore.Models;
using PCPartsStore.Services;

namespace PCPartsStore.ViewModels;

public partial class CartViewModel : ViewModelBase
{
    private readonly CartService _cartService;
    private readonly MainWindowViewModel _mainVm;

    [ObservableProperty]
    private ObservableCollection<CartItem> _items;

    [ObservableProperty]
    private decimal _total;

    public CartViewModel(CartService cartService, MainWindowViewModel mainVm)
    {
        _cartService = cartService;
        _mainVm = mainVm;
        _items = new ObservableCollection<CartItem>(_cartService.Items);
        _total = _cartService.Total;
    }

    private void RefreshItems()
    {
        Items = new ObservableCollection<CartItem>(_cartService.Items);
        Total = _cartService.Total;
    }

    [RelayCommand]
    private void RemoveItem(CartItem? item)
    {
        if (item is null) return;
        _cartService.Remove(item);
        RefreshItems();
    }

    [RelayCommand]
    private void IncreaseQuantity(CartItem? item)
    {
        if (item is null) return;
        if (item.Quantity >= item.Component.Stock) return;
        _cartService.UpdateQuantity(item, item.Quantity + 1);
        RefreshItems();
    }

    [RelayCommand]
    private void DecreaseQuantity(CartItem? item)
    {
        if (item is null) return;
        _cartService.UpdateQuantity(item, item.Quantity - 1);
        RefreshItems();
    }

    [RelayCommand]
    private void ProceedToCheckout()
    {
        if (!Items.Any()) return;
        _mainVm.NavigateToCheckout();
    }

    [RelayCommand]
    private void ContinueShopping()
    {
        _mainVm.NavigateToCatalog();
    }
}