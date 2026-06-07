using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using PCPartsStore.Data;
using PCPartsStore.Views;
using System.Globalization;

namespace PCPartsStore;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        CultureInfo.CurrentCulture = new CultureInfo("ro-RO");
        CultureInfo.CurrentUICulture = new CultureInfo("ro-RO");

        using (var context = new AppDbContext())
        {
            context.Database.EnsureCreated();
            context.SeedData();
        }

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}