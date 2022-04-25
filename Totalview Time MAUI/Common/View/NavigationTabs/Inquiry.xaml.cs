using Totalview_Time_Smartclient.Common.Services;

namespace Totalview_Time_Smartclient;

public partial class Inquiry : ContentPage
{
	public Inquiry()
	{
		InitializeComponent();

		AnalyticsService.Instance.TrackEvent(Event.Navigation, Category.PageSession, nameof(Inquiry));
	}
}