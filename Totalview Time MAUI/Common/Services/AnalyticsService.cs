using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Totalview_Time_MAUI.Common.Services;

internal interface IAnalyticsService
{
    void Start(bool trackDebugEvents = false);
    void TrackEvent(Event trackedEvent, Category trackedCategory, string trackedContent);
    void GenerateTestCrash();
}

internal class AnalyticsService : IAnalyticsService
{
    private static IAnalyticsService instance;

    public static IAnalyticsService Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AnalyticsService();
            }
            return instance;
        }
    }

    public void Start(bool trackDebugEvents = false)
    {
        if (Debugger.IsAttached && !trackDebugEvents)
        {
            return;
        }
#if IOS
        AppCenter.Start(AppSecret.Ios, typeof(Analytics), typeof(Crashes));
#elif ANDROID
        AppCenter.Start(AppSecret.Android, typeof(Analytics), typeof(Crashes));
#endif
    }

    public void TrackEvent(Event trackedEvent, Category trackedCategory, string trackedContent)
    {
        Dictionary<string, string> keyValuePairs = new()
        {
            { trackedCategory.ToString(), trackedContent }
        };
        Analytics.TrackEvent(trackedEvent.ToString(), keyValuePairs);
    }

    public void GenerateTestCrash()
    {
        Crashes.GenerateTestCrash();
    }
}

internal record AppSecret
{
    public const string Ios = "9ce1f24a-f676-4c1d-9f3b-ebfc5e5de6b1";
    public const string Android = "b70e447f-b954-49d8-8355-5fd68046774d";

}

internal enum Event
{
    [Display(Name = "Startup")]
    Startup,
    [Display(Name = "Navigation")]
    Navigation,
    [Display(Name = "Action")]
    Action,
    [Display(Name = "Error")]
    Error,
    [Display(Name = "Test")]
    Test,
    [Display(Name = "System")]
    System
}
internal enum Category
{
    // Startup Categories
    [Display(Name = "Locale")]
    Locale,
    [Display(Name = "Server Version")]
    ServerVersion,
    [Display(Name = "App Version Code")]
    AppVersionCode,
    [Display(Name = "App Build Version")]
    AppBuildVersion,

    // Navigation Categories
    [Display(Name = "Page Session")]
    PageSession,

    // Action Categories
    [Display(Name = "Touch")]
    Touch,
    [Display(Name = "Touch Long")]
    TouchLong,
    [Display(Name = "Swipe")]
    Swipe,

    // Error Categories
    [Display(Name = "Custom Error Message")]
    CustomErrorMessage,
    [Display(Name = "Error Message")]
    ErrorMessage,
    [Display(Name = "Error Type")]
    ErrorType,
    [Display(Name = "Exception Message")]
    ExceptionMessage,
    [Display(Name = "ExceptionType")]
    ExceptionType,

    // Test Categories
    [Display(Name = "Analytics Test")]
    AnalyticsTest,

    // System Categories
    [Display(Name = "Automatic Logout")]
    AutomaticLogout,
    [Display(Name = "Automatic Login")]
    AutomaticLogin,
    [Display(Name = "Automatic Navigation")]
    AutomaticNavigation,
}
