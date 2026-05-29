using CommunityToolkit.Mvvm.Input;

namespace PCPartsStore.ViewModels;

public partial class RoleSelectionViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _mainVm;

    public RoleSelectionViewModel(MainWindowViewModel mainVm)
    {
        _mainVm = mainVm;
    }

    [RelayCommand]
    private void SelectCustomer()
    {
        _mainVm.SelectCustomer();
    }

    [RelayCommand]
    private void SelectAdmin()
    {
        _mainVm.SelectAdmin();
    }
}