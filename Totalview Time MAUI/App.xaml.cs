using Totalview_Time_MAUI.Common.Services;

namespace Totalview_Time_MAUI;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        AnalyticsService.Instance.Start(true);
        AnalyticsService.Instance.TrackEvent(Event.Test, Category.AnalyticsTest, "Test");

        MainPage = new AppShell();
    }
}
