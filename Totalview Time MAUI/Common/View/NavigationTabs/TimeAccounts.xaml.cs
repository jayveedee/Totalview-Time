using Totalview_Time_MAUI.Common.Services;

namespace Totalview_Time_MAUI;

public partial class TimeAccounts : ContentPage
{
	public TimeAccounts()
	{
		InitializeComponent();

		AnalyticsService.Instance.TrackEvent(Event.Navigation, Category.PageSession, nameof(TimeAccounts));
	}
}