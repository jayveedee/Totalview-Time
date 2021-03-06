using CommunityToolkit.Mvvm.Input;
using Totalview_Time_MAUI.Common.Services;
using Totalview_Time_MAUI.Common.Services.Authentication;

namespace Totalview_Time_MAUI.Common.ViewModel;

public partial class AppShellViewModel : BaseViewModel
{
    [ICommand]
    public async void Logout()
    {
        AnalyticsService.Instance.TrackEvent(Event.Action, Category.Touch, "Logout tapped");

        var answer = await Application.Current.MainPage.DisplayAlert("Log out?", "Are you sure you want to log out?", "Yes", "No");
        if (!answer)
        {
            return;
        }

        var values = StorageService.Instance.LoadStorage();

        if (values.OidcOptions == null || values.AuthCredentials == null)
        {
            values = await StorageService.Instance.LoadStorageAsync();
        }

        if (values.OidcOptions != null && values.AuthCredentials != null)
        {
            var authService = new AuthenticationService(values.OidcOptions);
            var credentials = await authService.SignOut(values.AuthCredentials);

            if (!credentials.IsValid)
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginServerDetails)}");
            } 
        }
    }
    public async void DetermineLoginRoute()
    {
        var values = await StorageService.Instance.LoadStorageAsync();
        if (values == null || values.AuthCredentials == null)
        {
            return;
        }
        if (values.AuthCredentials?.AccessTokenExpiration > DateTime.Now)
        {
            AnalyticsService.Instance.TrackEvent(Event.System, Category.AutomaticNavigation, "LoginSuceededViaUpToDateCredentials navigating to MainPage");
            await Shell.Current.GoToAsync($"//{nameof(TimeAccounts)}");
        }
        else if (values.OidcOptions != null && values.AuthCredentials?.AccessToken != null)
        {
            var authService = new AuthenticationService(values.OidcOptions);
            var credential = await authService.RefreshSession(values.AuthCredentials);

            if (credential.AccessToken != null)
            {
                AnalyticsService.Instance.TrackEvent(Event.System, Category.AutomaticNavigation, "LoginSuceededViaRefreshedCredentials navigating to MainPage");
                await Shell.Current.GoToAsync($"//{nameof(TimeAccounts)}");
            }
        }
    }
}
