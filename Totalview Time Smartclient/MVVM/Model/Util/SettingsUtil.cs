namespace Totalview_Time_Smartclient.MVVM.Model.Util
{
    internal static class SettingsUtil
    {
        // Aanalytics
        public const string App_secret_ios = "9ce1f24a-f676-4c1d-9f3b-ebfc5e5de6b1";
        public const string App_secret_android = "b70e447f-b954-49d8-8355-5fd68046774d";

        // Authentication (Only thing here subject to change is authority)
        public const string CallbackUri = "fo.formula.totalview";
        public static string Authority = "https://tvdev.formula.fo/TotalviewAuthentication";
        public const string CliendId = "totalview-maui-totalview-time-client";
        public const string ClientName = "Totalview MAUI Totalview Time client";
        public const string Scope = "openid profile totalview-time-server offline_access";
        public const string RedirectUri = $"{CallbackUri}://oauth2redirect";
        public const string PostLogourUri = $"{CallbackUri}://oauth2redirect";
        public const string ClientSecret = null;

        // Server
        public static string ServerAddressHTTPS = "https://dev-tv.formula.fo:9000/TVWCF";
        public static string ServerAddressTCP = "net.tcp://dev-tv.formula.fo:9001/TVWCF";
     
    }
}
