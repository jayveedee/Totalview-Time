using Totalview_Time_MAUI.Common.Services;
using Totalview_Time_MAUI.Common.ViewModel.NavigationTabs;

namespace Totalview_Time_MAUI;

public partial class TimeAccounts : ContentPage
{
	private readonly TimeAccountsViewModel viewModel;
	public TimeAccounts()
	{
		InitializeComponent();

		viewModel = new TimeAccountsViewModel();
		BindingContext = viewModel;

		AnalyticsService.Instance.TrackEvent(Event.Navigation, Category.PageSession, nameof(TimeAccounts));
	}
}