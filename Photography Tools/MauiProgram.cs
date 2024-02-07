﻿using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Photography_Tools.Pages;
using Photography_Tools.Services.PhotographyCalculationsService;
using Photography_Tools.ViewModels;

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

        // Pages
        builder.Services.AddSingleton<DofCalcPage>();
        builder.Services.AddSingleton<NdFilterCalcPage>();

        // Services
        builder.Services.AddSingleton<IPhotographyCalculationsService, PhotographyCalculationsService>();

        // ViewModel
        builder.Services.AddSingleton<DofCalcViewModel>();

        return builder.Build();
    }
}
