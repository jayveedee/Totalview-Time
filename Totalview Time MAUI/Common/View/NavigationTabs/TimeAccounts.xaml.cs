using Totalview_Time_Smartclient.Common.Services;

namespace Totalview_Time_Smartclient;

public partial class TimeAccounts : ContentPage
{
	public TimeAccounts()
	{
		InitializeComponent();

		AnalyticsService.Instance.TrackEvent(Event.Navigation, Category.PageSession, nameof(TimeAccounts));
	}
}