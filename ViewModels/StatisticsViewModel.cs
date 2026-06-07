using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using PCPartsStore.Services;

namespace PCPartsStore.ViewModels;

public class CategoryRevenue
{
    public string Category { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
}

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
    private List<CategoryRevenue> _revenueByCategoryList = new();

    public StatisticsViewModel(OrderService orderService)
    {
        TotalSalesToday = orderService.GetTotalSalesToday();
        TotalSalesThisWeek = orderService.GetTotalSalesThisWeek();
        TotalSalesThisMonth = orderService.GetTotalSalesThisMonth();
        TotalSalesThisYear = orderService.GetTotalSalesThisYear();

        var revenueByCategory = orderService.GetRevenueByCategory();
        RevenueByCategoryList = revenueByCategory
            .Select(kvp => new CategoryRevenue { Category = kvp.Key, Revenue = kvp.Value })
            .OrderByDescending(x => x.Revenue)
            .ToList();
    }
}