using Totalview_Time_Smartclient.MVVM.Model.Services;
using Totalview_Time_Smartclient.MVVM.ViewModel.Login;

namespace Totalview_Time_Smartclient;

public partial class LoginUserDetails : ContentPage
{
    private readonly LoginUserDetailsViewModel viewModel;
	public LoginUserDetails()
	{
		InitializeComponent();

        viewModel = new LoginUserDetailsViewModel();
        BindingContext = viewModel;

        AnalyticsService.Instance.TrackEvent(Event.Navigation, Category.PageSession, nameof(LoginUserDetails));

        UsernameEntry.Completed += (s, e) =>
        {
            UsernameEntry.Unfocus();
            PasswordEntry.Focus();
        };
        PasswordEntry.Completed += (s, e) =>
        {
            PasswordEntry.Unfocus();
            viewModel.LoginToServer();
        };
        PasswordVisibilityButton.Pressed += (s, e) =>
        {
            PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
        };
    }
}