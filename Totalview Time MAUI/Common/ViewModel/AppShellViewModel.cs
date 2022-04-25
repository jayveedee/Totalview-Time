using CommunityToolkit.Mvvm.Input;
using Totalview_Time_Smartclient.Common.Services;
using Totalview_Time_Smartclient.Common.Services.Authentication;

namespace Totalview_Time_Smartclient.Common.ViewModel;

public partial class AppShellViewModel : BaseViewModel
{
    [ICommand]
    public async void Logout()
    {
        AnalyticsService.Instance.TrackEvent(Event.Action, Category.Touch, "Logout tapped");
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
        var authService = new AuthenticationService(values.OidcOptions);
        IRefreshSessionService refreshSessionService = new RefreshSessionService(authService);
        if (values.AuthCredentials?.AccessTokenExpiration > DateTime.Now)
        {
            await Shell.Current.GoToAsync($"//{nameof(TimeAccounts)}");
        }
        else if (values.OidcOptions != null)
        {
            var credential = await authService.RefreshSession(values.AuthCredentials);

            if (credential.AccessToken != null)
            {
                await Shell.Current.GoToAsync($"//{nameof(TimeAccounts)}");
            }
        }
        refreshSessionService.StartRefreshing(values.AuthCredentials);
    }
}
