using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using Totalview_Time_MAUI.Common.Services;
using Totalview_Time_MAUI.Common.Services.Authentication;
using Totalview_Time_MAUI.Common.Util;

namespace Totalview_Time_MAUI.Common.ViewModel.Login;

public partial class LoginServerDetailsViewModel : BaseViewModel
{
    [ObservableProperty]
    private string serverAddressValue;
    [ObservableProperty]
    private string authorityServerAddressValue;

    [ICommand]
    public async void ConnectToServer()
    {
        AnalyticsService.Instance.TrackEvent(Event.Action, Category.Touch, "ConnectToServer tapped");

        OidcOptions openIdOptions;
        try
        {
            // Defualt server to login
            if (String.IsNullOrEmpty(ServerAddressValue) || String.IsNullOrEmpty(AuthorityServerAddressValue))
            {
                openIdOptions = new OidcOptions();
            }
            else
            {
                openIdOptions = new OidcOptions(BudgetDiscoveryUtil.formatAuthorityAddress(AuthorityServerAddressValue));
            }
            var storage = StorageService.Instance.LoadStorage();
            storage.OidcOptions = openIdOptions;
            StorageService.Instance.SaveStorage(storage);
        }
        catch (Exception ex)
        {
            Debug.Write(ex.ToString());
            await Application.Current.MainPage.DisplayAlert("Error", "Could not connect to server", "OK");
            return;
        }
        
        IAuthenticationService authenticationService = new AuthenticationService(openIdOptions);
        AuthCredentials credentials = await authenticationService.SignIn();

        if (credentials.AccessToken == null)
        {
            Debug.Write(credentials.ToString());
            await Application.Current.MainPage.DisplayAlert("Error", "Authentication failed", "OK");
            return;
        }

        await Shell.Current.GoToAsync($"//{nameof(TimeAccounts)}");
    }
}
