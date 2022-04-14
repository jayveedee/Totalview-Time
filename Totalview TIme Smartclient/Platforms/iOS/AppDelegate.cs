using Foundation;
using UIKit;

namespace Totalview_Time_Smartclient;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options) =>
            Platform.OpenUrl(app, url, options) 
            || base.OpenUrl(app, url, options);

        public override bool ContinueUserActivity(UIApplication application, NSUserActivity userActivity, UIApplicationRestorationHandler completionHandler) =>
            Platform.ContinueUserActivity(application, userActivity, completionHandler) 
            || base.ContinueUserActivity(application, userActivity, completionHandler);
}
