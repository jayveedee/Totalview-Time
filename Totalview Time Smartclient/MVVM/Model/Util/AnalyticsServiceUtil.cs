using System.ComponentModel.DataAnnotations;

namespace Totalview_Time_Smartclient.MVVM.Model.Util
{
    public static class AnalyticsServiceUtil
    {
        public enum Event
        {
            [Display(Name = "Startup")]
            Startup,
            [Display(Name = "Navigation")]
            Navigation,
            [Display(Name = "Action")]
            Action,
            [Display(Name = "Error")]
            Error
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
            ExceptionType
        }
    }
}
