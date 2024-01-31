using CommunityToolkit.Maui.Core;
using Microsoft.Extensions.Logging;
using Photography_Tools.Pages;
using Photography_Tools.Services.DofService;

namespace Photography_Tools;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif
        builder.UseMauiApp<App>().UseMauiCommunityToolkitCore();

        //Pages
        builder.Services.AddSingleton<DofCalcPage>();
        builder.Services.AddSingleton<NdFilterCalcPage>();

        // Services
        builder.Services.AddSingleton<IDofService, DofService>();

        return builder.Build();
    }
}
