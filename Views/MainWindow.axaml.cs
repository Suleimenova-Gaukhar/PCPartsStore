using Avalonia.Controls;
using PCPartsStore.ViewModels;

namespace PCPartsStore.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }
}