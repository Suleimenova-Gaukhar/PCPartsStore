using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using PCPartsStore.Services;

namespace PCPartsStore.ViewModels;

public partial class StatisticsViewModel : ViewModelBase
{
    [ObservableProperty]
    private decimal _totalSalesToday;

    [ObservableProperty]
    private decimal _totalSalesThisWeek;

    [ObservableProperty]
    private decimal _totalSalesThisMonth;

    [ObservableProperty]
    private decimal _totalSalesThisYear;

    [ObservableProperty]
    private Dictionary<string, decimal> _revenueByCategory = new();

    public StatisticsViewModel(OrderService orderService)
    {
        TotalSalesToday = orderService.GetTotalSalesToday();
        TotalSalesThisWeek = orderService.GetTotalSalesThisWeek();
        TotalSalesThisMonth = orderService.GetTotalSalesThisMonth();
        TotalSalesThisYear = orderService.GetTotalSalesThisYear();
        RevenueByCategory = orderService.GetRevenueByCategory();
    }
}