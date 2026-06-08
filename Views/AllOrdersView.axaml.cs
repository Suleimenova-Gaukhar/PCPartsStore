using Avalonia.Controls;
using Avalonia.Input;
using PCPartsStore.ViewModels;

namespace PCPartsStore.Views;

public partial class AllOrdersView : UserControl
{
    public AllOrdersView()
    {
        InitializeComponent();
    }

    private void OnOrderRowPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is Border border &&
            border.DataContext is OrderRow row &&
            DataContext is AllOrdersViewModel vm)
        {
            vm.SelectedOrderRow = row;
        }
    }
}