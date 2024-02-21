using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace Photography_Tools;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        //DataAccess
        builder.Services.AddSingleton<INDFiltersDataAccess, StaticNDFiltersDataAccess>();
        builder.Services.AddSingleton<ISensorsDataAccess, StaticSensorsDataAccess>();

        // Pages
        builder.Services.AddSingleton<AstroTimeCalcPage>();
        builder.Services.AddSingleton<DofCalcPage>();
        builder.Services.AddSingleton<NdFilterCalcPage>();

        // Services
        builder.Services.AddSingleton<IPhotographyCalculationsService, PhotographyCalculationsService>();

        // ViewModel
        builder.Services.AddSingleton<AstroTimeCalcViewModel>();
        builder.Services.AddSingleton<DofCalcViewModel>();
        builder.Services.AddSingleton<NDFilterCalcViewModel>();

        return builder.Build();
    }
}
