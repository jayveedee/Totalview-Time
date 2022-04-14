using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Totalview_Time_Smartclient.MVVM.Model.Util;

namespace Totalview_Time_Smartclient.MVVM.Model.Services;

public interface IAnalyticsService
{
    void Start(bool trackDebugEvents = false);
    void TrackEvent(Event trackedEvent, Category trackedCategory, string trackedContent);
    void GenerateTestCrash();
}

public class AnalyticsService : IAnalyticsService
{
    private static IAnalyticsService instance;
    private readonly AppSecret _appSecret;

    private AnalyticsService()
    {
        _appSecret = new AppSecret(SettingsUtil.App_secret_ios, SettingsUtil.App_secret_android);
    }

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
        AppCenter.Start(_appSecret.Ios, typeof(Analytics), typeof(Crashes));
#elif ANDROID
        AppCenter.Start(_appSecret.Android, typeof(Analytics), typeof(Crashes));
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

public record AppSecret
{
    public readonly string Ios;
    public readonly string Android;

    public AppSecret(string ios, string android)
    {
        Ios = ios ?? throw new ArgumentNullException(nameof(ios));
        Android = android ?? throw new ArgumentNullException(nameof(android));
    }
}

public enum Event
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
    Test
}
public enum Category
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
    AnalyticsTest
}
