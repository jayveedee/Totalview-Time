using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;
using Totalview_Time_MAUI.Common.Model.TimeRegistration;
using Totalview_Time_MAUI.Common.Services;
using CommunityToolkit.Maui.Views;

namespace Totalview_Time_MAUI.Common.ViewModel.NavigationTabs.StateDetails;

[QueryProperty(nameof(Registration), nameof(Registration))]
internal partial class StateDetailsViewModel : BaseViewModel
{
    [ObservableProperty]
    public Registration registration;
    public bool ShowAddButton
    {
        get => showAddButton;
        set => SetProperty(ref showAddButton, value);
    }
    private bool showAddButton;

    public StateDetailsViewModel()
    {
        ShowAddButton = false;
    }

    [ICommand]
    public async void EditRegistration()
    {
        AnalyticsService.Instance.TrackEvent(Event.Action, Category.Touch, "EditRegistration tapped");
        if (registration.Accessibility == EditState.Locked)
        {
            await Application.Current.MainPage.DisplayAlert("Registration locked", "Cannot edit a locked registration", "OK");
            return;
        }
        ShowAddButton = !ShowAddButton;
    }

    public void ShowEditStatePopup(ContentPage page)
    {
        AnalyticsService.Instance.TrackEvent(Event.Action, Category.Touch, "EditStatePopup tapped");
        var popup = new EditStatePopup(registration);
        page.ShowPopup(popup);
    }
}
