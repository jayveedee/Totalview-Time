﻿using Totalview_Time_Smartclient.Common.Services;

namespace Totalview_Time_Smartclient;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        IConfigurationService configService = new ConfigurationService();

        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts = configService.ConfigureAppFonts(fonts);
            });

        return builder.Build();
    }
}