﻿using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Photography_Tools.DataAccess.AstroDataAccess;
using Photography_Tools.Services.KeyValueStoreService;

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

        // DataAccess
        builder.Services.AddSingleton<IAstroDataAccess, UsnoAstroDataAccess>();
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
        builder.Services.AddKeyedSingleton<IAstroDataService, OfflineAstroDataService>("offlineAstroData");
        builder.Services.AddKeyedSingleton<IAstroDataService, OnlineAstroDataService>("onlineAstroData");
        builder.Services.AddSingleton<IKeyValueStore<AstroData>, CacheKeyValueStore<AstroData>>();
        builder.Services.AddSingleton<IPhotographyCalculationsService, PhotographyCalculationsService>();
        builder.Services.AddSingleton<IPreferencesService, PreferencesService>();

        // ViewModel
        builder.Services.AddSingleton<AstroTimeCalcViewModel>();
        builder.Services.AddSingleton<DofCalcViewModel>();
        builder.Services.AddSingleton<NDFilterCalcViewModel>();
        builder.Services.AddSingleton<MoonPhaseViewModel>();
        builder.Services.AddSingleton<TimeLapseCalculatorViewModel>();

        // HttpClient
        builder.Services.AddHttpClient<IAstroDataAccess, UsnoAstroDataAccess>(client =>
        {
            client.BaseAddress = new Uri("https://aa.usno.navy.mil/api/");
        });

        return builder.Build();
    }
}
