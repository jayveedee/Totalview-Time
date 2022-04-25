using Totalview_Time_Smartclient.Common.Services;
using Totalview_Time_Smartclient.Common.ViewModel.Login;

namespace Totalview_Time_Smartclient;

public partial class LoginServerDetails : ContentPage
{
	private readonly LoginServerDetailsViewModel viewModel;
	public LoginServerDetails()
	{
		InitializeComponent();

		viewModel = new LoginServerDetailsViewModel();
		BindingContext = viewModel;

		AnalyticsService.Instance.TrackEvent(Event.Navigation, Category.PageSession, nameof(LoginServerDetails));

        GoToNextEntryHandler();
    }

    private void GoToNextEntryHandler()
    {
        ServerAddressEntry.Completed += (s, e) =>
        {
            ServerAddressEntry.Unfocus();
            AuthorityServerAddressEntry.Focus();
        };
        AuthorityServerAddressEntry.Completed += (s, e) =>
        {
            AuthorityServerAddressEntry.Unfocus();
            viewModel.ConnectToServer();
        };
    }
}