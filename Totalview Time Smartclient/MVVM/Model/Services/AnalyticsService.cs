using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System.Diagnostics;
using Totalview_Time_Smartclient.MVVM.Model.Util;

namespace Totalview_Time_Smartclient.MVVM.Model.Services;

public interface IAnalyticsService
{
    void Start(bool trackDebugEvents = false);
    void TrackEvent(AnalyticsServiceUtil.Event trackedEvent, AnalyticsServiceUtil.Category trackedCategory, string trackedContent);
    void GenerateTestCrash();
}

public class AnalyticsService : IAnalyticsService
{
    private readonly string _app_secret_ios = "9ce1f24a-f676-4c1d-9f3b-ebfc5e5de6b1";
    private readonly string _app_secret_android = "b70e447f-b954-49d8-8355-5fd68046774d";

    public AnalyticsService() { }

    public void Start(bool trackDebugEvents = false)
    {           
        if (Debugger.IsAttached && !trackDebugEvents)
        {
            return;
        }
#if IOS
        AppCenter.Start(_app_secret_ios, typeof(Analytics), typeof(Crashes));
#elif ANDROID
        AppCenter.Start(_app_secret_android, typeof(Analytics), typeof(Crashes));
#endif
    }

    public void TrackEvent(AnalyticsServiceUtil.Event trackedEvent, AnalyticsServiceUtil.Category trackedCategory, string trackedContent)
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
