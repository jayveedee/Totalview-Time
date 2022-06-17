using Totalview_Time_MAUI.Common.Services;
using CommunityToolkit.Maui.Markup;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui;

namespace Totalview_Time_MAUI;

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
        builder.UseMauiApp<App>().UseMauiCommunityToolkitMarkup();
        builder.UseMauiApp<App>().UseMauiCommunityToolkitCore();
        builder.UseMauiApp<App>().UseMauiCommunityToolkit();


        return builder.Build();
    }
}
