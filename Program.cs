using System.Globalization;
using Avalonia;
using PCPartsStore;

CultureInfo.CurrentCulture = new CultureInfo("ro-RO");
CultureInfo.CurrentUICulture = new CultureInfo("ro-RO");

BuildAvaloniaApp()
    .StartWithClassicDesktopLifetime(args);

static AppBuilder BuildAvaloniaApp()
    => AppBuilder.Configure<App>()
        .UsePlatformDetect()
        .WithInterFont()
        .LogToTrace();