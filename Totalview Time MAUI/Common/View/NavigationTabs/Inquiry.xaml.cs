using Totalview_Time_MAUI.Common.Services;
using Totalview_Time_MAUI.Common.ViewModel.NavigationTabs;

namespace Totalview_Time_MAUI;

public partial class Inquiry : ContentPage
{
	private readonly InquiryViewModel viewModel;
	public Inquiry()
	{
		InitializeComponent();

		viewModel = new InquiryViewModel();
		BindingContext = viewModel;

		AnalyticsService.Instance.TrackEvent(Event.Navigation, Category.PageSession, nameof(Inquiry));
	}
}