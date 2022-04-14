using System.Diagnostics;
using System.Windows.Input;
using Totalview_Time_Smartclient.MVVM.Model.Services;
using Totalview_Time_Smartclient.MVVM.Model.Services.Authentication;
using Totalview_Time_Smartclient.MVVM.Model.Services.WebService;
using Totalview_Time_Smartclient.MVVM.Model.Util;

namespace Totalview_Time_Smartclient.MVVM.ViewModel.Login;

public class LoginServerDetailsViewModel : BaseViewModel
{
    public string ServerAddressValue { get; set; }
    public string AuthenticationEndpointValue { get; set; }
    public ICommand ConnectToServerCommand { get; set; }

    public LoginServerDetailsViewModel() : base()
    {
        ConnectToServerCommand = new Command(ConnectToServer);
    }

    private async void ConnectToServer()
    {
        AnalyticsService.Instance.TrackEvent(Event.Action, Category.Touch, "ConnectToServer tapped");

        IAuthenticationService authService;
        try
        {
            // Defualt server to login
            if (String.IsNullOrEmpty(ServerAddressValue) && String.IsNullOrEmpty(AuthenticationEndpointValue))
            {
                authService = new AuthenticationService(new OpenIdOptions(
                    SettingsUtil.Authority,
                    SettingsUtil.CliendId,
                    SettingsUtil.Scope,
                    SettingsUtil.RedirectUri,
                    SettingsUtil.PostLogourUri,
                    SettingsUtil.ClientSecret));
            }
            else
            {
                authService = new AuthenticationService(new OpenIdOptions(
                    AuthenticationEndpointValue,
                    SettingsUtil.CliendId,
                    SettingsUtil.Scope,
                    SettingsUtil.RedirectUri,
                    SettingsUtil.PostLogourUri,
                    SettingsUtil.ClientSecret));
            }

        }
        catch (Exception ex)
        {
            Debug.Write(ex.ToString());
            await Application.Current.MainPage.DisplayAlert("Error", "Could not connect to server", "OK");
            return;
        }
        

        AuthenticationCredentials credentials = await authService.SignIn();
        //AuthenticationCredentials logoutCredentials = await authService.SignOut(credentials);
    }
}
