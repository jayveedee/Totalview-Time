using Totalview_Time_Smartclient.Common.Services;
using Totalview_Time_Smartclient.Common.ViewModel.Login;

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

        GoToNextEntryHandler();
        PasswordVisibilityHandler();
    }

    private void GoToNextEntryHandler()
    {
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
    }

    private void PasswordVisibilityHandler()
    {
        PasswordVisibilityButton.Pressed += (s, e) =>
        {
            PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
        };
    }
}