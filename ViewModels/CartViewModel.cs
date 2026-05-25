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
    private CartItem? _selectedItem;

    [ObservableProperty]
    private decimal _total;

    public CartViewModel(CartService cartService, MainWindowViewModel mainVm)
    {
        _cartService = cartService;
        _mainVm = mainVm;
        _items = new ObservableCollection<CartItem>(_cartService.Items);
        _total = _cartService.Total;
    }

    [RelayCommand]
    private void RemoveItem()
    {
        if (SelectedItem is null) return;
        _cartService.Remove(SelectedItem);
        Items.Remove(SelectedItem);
        Total = _cartService.Total;
    }

    [RelayCommand]
    private void IncreaseQuantity()
    {
        if (SelectedItem is null) return;
        _cartService.UpdateQuantity(SelectedItem, SelectedItem.Quantity + 1);
        Total = _cartService.Total;
    }

    [RelayCommand]
    private void DecreaseQuantity()
    {
        if (SelectedItem is null) return;
        _cartService.UpdateQuantity(SelectedItem, SelectedItem.Quantity - 1);
        Items = new ObservableCollection<CartItem>(_cartService.Items);
        Total = _cartService.Total;
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