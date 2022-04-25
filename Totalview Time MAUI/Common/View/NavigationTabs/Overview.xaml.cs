using Totalview_Time_Smartclient.Common.Services;

namespace Totalview_Time_Smartclient;

public partial class Overview : ContentPage
{
	public Overview()
	{
		InitializeComponent();

		AnalyticsService.Instance.TrackEvent(Event.Navigation, Category.PageSession, nameof(Overview));
	}
}