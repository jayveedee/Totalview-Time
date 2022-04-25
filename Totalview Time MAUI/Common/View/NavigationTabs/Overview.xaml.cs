using Totalview_Time_MAUI.Common.Services;

namespace Totalview_Time_MAUI;

public partial class Overview : ContentPage
{
	public Overview()
	{
		InitializeComponent();

		AnalyticsService.Instance.TrackEvent(Event.Navigation, Category.PageSession, nameof(Overview));
	}
}