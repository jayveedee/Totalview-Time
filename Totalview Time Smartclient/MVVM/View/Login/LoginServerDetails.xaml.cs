using Totalview_Time_Smartclient.MVVM.Model.Services;

namespace Totalview_Time_Smartclient;

public partial class LoginServerDetails : ContentPage
{
	public LoginServerDetails()
	{
		InitializeComponent();

		AnalyticsService.Instance.TrackEvent(Event.Navigation, Category.PageSession, nameof(LoginServerDetails));
	}
}