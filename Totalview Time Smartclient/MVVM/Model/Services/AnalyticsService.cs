using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System.Diagnostics;
using Totalview_Time_Smartclient.MVVM.Model.Util;

namespace Totalview_Time_Smartclient.MVVM.Model.Services
{
    public interface IAnalyticsService
    {
        void Start(bool trackDebugEvents);
        void TrackEvent(AnalyticsServiceUtil.Event trackedEvent, AnalyticsServiceUtil.Category trackedCategory, string trackedContent);
    }

    public class AnalyticsService : IAnalyticsService
    {
        private readonly string _app_secret = "0f78a403-b6ea-4fe6-bc7e-d6c47c9b381c";

        public AnalyticsService() { }

        public void Start(bool trackDebugEvents)
        {           
            if (Debugger.IsAttached && !trackDebugEvents)
            {
                return;
            }
            AppCenter.Start(_app_secret, typeof(Analytics), typeof(Crashes));
        }

        public void TrackEvent(AnalyticsServiceUtil.Event trackedEvent, AnalyticsServiceUtil.Category trackedCategory, string trackedContent)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            keyValuePairs.Add(trackedCategory.ToString(), trackedContent);
            Analytics.TrackEvent(trackedEvent.ToString(), keyValuePairs);
        }
    }
}
