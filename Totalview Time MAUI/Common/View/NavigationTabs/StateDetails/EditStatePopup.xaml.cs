using CommunityToolkit.Maui.Views;
using Totalview_Time_MAUI.Common.Services;
using Totalview_Time_MAUI.Common.Model.TimeManagement;
using Totalview_Time_MAUI.Common.ViewModel.NavigationTabs.StateDetails;

namespace Totalview_Time_MAUI;

public partial class EditStatePopup : Popup
{
	private EditStatePopupViewModel viewModel;
	public EditStatePopup(TimeRegistration registration)
	{
		InitializeComponent();
		viewModel = new EditStatePopupViewModel(registration, this);
		BindingContext = viewModel;

        AnalyticsService.Instance.TrackEvent(Event.Navigation, Category.PageSession, nameof(EditStatePopup));
    }
}