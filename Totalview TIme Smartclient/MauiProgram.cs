using Totalview_Time_Smartclient.MVVM.Model.Services;
using Totalview_Time_Smartclient.MVVM.Model.Util;

namespace Totalview_Time_Smartclient;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        IAppConfigurationService configService = new AppConfigurationService();
        IAnalyticsService analyticsService = new AnalyticsService();

        analyticsService.Start(true);
        analyticsService.TrackEvent(AnalyticsServiceUtil.Event.Test, AnalyticsServiceUtil.Category.AnalyticsTest, "Test");

        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                configService.AddFonts(fonts);
            });

        return builder.Build();
    }
}
