using Android.App;
using Android.Content.PM;
using Totalview_Time_Smartclient.MVVM.Model.Util;

namespace Totalview_Time_Smartclient;

[Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
[IntentFilter(new[] { Android.Content.Intent.ActionView }, Categories = new[] { Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable }, DataScheme = SettingsUtil.CallbackUri)]
internal class AuthenticationActivity : WebAuthenticatorCallbackActivity
{

}