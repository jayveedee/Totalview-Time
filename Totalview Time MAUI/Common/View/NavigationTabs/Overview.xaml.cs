using Totalview_Time_MAUI.Common.Services;
using Totalview_Time_MAUI.Common.ViewModel.NavigationTabs;

namespace Totalview_Time_MAUI;

public partial class Overview : ContentPage
{
	private readonly OverviewViewModel viewModel;
	public Overview()
	{
		InitializeComponent();

		viewModel = new OverviewViewModel();
		BindingContext = viewModel;

		AnalyticsService.Instance.TrackEvent(Event.Navigation, Category.PageSession, nameof(Overview));

		listView.ItemTapped += (object sender, ItemTappedEventArgs e) => {
			// don't do anything if we just de-selected the row.
			if (e.Item == null) return;

			if (sender is ListView lv) lv.SelectedItem = null;

			viewModel.ItemTappedCommand(e.Item);
		};
	}
}