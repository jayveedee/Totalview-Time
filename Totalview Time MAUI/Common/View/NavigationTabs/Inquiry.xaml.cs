using Totalview_Time_MAUI.Common.Services;

namespace Totalview_Time_MAUI;

public partial class Inquiry : ContentPage
{
	public Inquiry()
	{
		InitializeComponent();

		AnalyticsService.Instance.TrackEvent(Event.Navigation, Category.PageSession, nameof(Inquiry));
	}
}