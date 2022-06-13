using Totalview_Time_MAUI.Common.Services;
using Totalview_Time_MAUI.Common.ViewModel.NavigationTabs.InquiryDetails;

namespace Totalview_Time_MAUI;

public partial class InProgress : ContentPage
{
    private readonly InProgressViewModel viewModel;
	public InProgress()
	{
		InitializeComponent();

        viewModel = new InProgressViewModel();
        BindingContext = viewModel;

        listView.ItemTapped += (object sender, ItemTappedEventArgs e) => {
            // don't do anything if we just de-selected the row.
            if (e.Item == null) return;

            if (sender is ListView lv) lv.SelectedItem = null;

            viewModel.ItemTappedCommand(e.Item);
        };

        AnalyticsService.Instance.TrackEvent(Event.Navigation, Category.PageSession, nameof(InProgress));
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        viewModel.UpdateList();
    }
}