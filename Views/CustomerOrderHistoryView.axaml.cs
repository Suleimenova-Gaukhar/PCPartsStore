using Avalonia.Controls;
using Avalonia.Input;
using PCPartsStore.Models;
using PCPartsStore.ViewModels;

namespace PCPartsStore.Views;

public partial class CustomerOrderHistoryView : UserControl
{
    public CustomerOrderHistoryView()
    {
        InitializeComponent();
    }

    private void OnOrderRowPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is Border border &&
            border.DataContext is Order order &&
            DataContext is CustomerOrderHistoryViewModel vm)
        {
            vm.SelectedOrder = order;
        }
    }
}