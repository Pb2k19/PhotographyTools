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
        builder.Services.AddSingleton(Preferences.Default);

        // Pages
        builder.Services.AddSingleton<AstroTimeCalcPage>();
        builder.Services.AddSingleton<DofCalcPage>();
        builder.Services.AddSingleton<NdFilterCalcPage>();
        builder.Services.AddSingleton<MoonPhasePage>();
        builder.Services.AddSingleton<TimeLapseCalcPage>();

        // Services
        builder.Services.AddSingleton<IPhotographyCalculationsService, PhotographyCalculationsService>();
        builder.Services.AddSingleton<IPreferencesService, PreferencesService>();

        // ViewModel
        builder.Services.AddSingleton<AstroTimeCalcViewModel>();
        builder.Services.AddSingleton<DofCalcViewModel>();
        builder.Services.AddSingleton<NDFilterCalcViewModel>();
        builder.Services.AddSingleton<MoonPhaseViewModel>();
        builder.Services.AddSingleton<TimeLapseCalculatorViewModel>();

        return builder.Build();
    }
}
