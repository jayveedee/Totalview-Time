using CommunityToolkit.Maui.Views;
using Totalview_Time_MAUI.Common.Services;
using Totalview_Time_MAUI.Common.ViewModel.NavigationTabs.StateDetails;

namespace Totalview_Time_MAUI;

public partial class StateDetails : ContentPage
{
	private readonly StateDetailsViewModel viewModel;
	public StateDetails()
	{
		InitializeComponent();
		viewModel = new StateDetailsViewModel();
		BindingContext = viewModel;

		AnalyticsService.Instance.TrackEvent(Event.Navigation, Category.PageSession, nameof(StateDetails));

		listView.ItemTapped += (object sender, ItemTappedEventArgs e) => {
			// don't do anything if we just de-selected the row.
			if (e.Item == null) return;

			if (sender is ListView lv) lv.SelectedItem = null;
		};
		listView.Scrolled += (object sender, ScrolledEventArgs e) =>
		{
			var listView = (ListView)sender;
			var firstItem = listView.ItemsSource.OfType<object>().First();
			listView.ScrollTo(firstItem, ScrollToPosition.Start, false);
		};
		addButton.Clicked += (object sender, EventArgs e) =>
        {
			viewModel.ShowEditStatePopup(this);
        };
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

		if (viewModel.registration.Accessibility == Common.Model.TimeRegistration.EditState.Open)
        {
			LockLockedImage.IsVisible = false;
			editDisabledImageButton.IsVisible = false;
			LockOpenedImage.IsVisible = true;
			editEnabledImageButton.IsVisible = true;
        } 
		else
        {
            LockLockedImage.IsVisible = true;
            editDisabledImageButton.IsVisible = true;
            LockOpenedImage.IsVisible = false;
            editEnabledImageButton.IsVisible = false;
        }
    }
}