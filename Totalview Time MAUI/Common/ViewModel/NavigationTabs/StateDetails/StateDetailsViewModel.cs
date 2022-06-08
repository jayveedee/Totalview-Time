using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;
using Totalview_Time_MAUI.Common.Model.TimeManagement;
using Totalview_Time_MAUI.Common.Services;
using CommunityToolkit.Maui.Views;

namespace Totalview_Time_MAUI.Common.ViewModel.NavigationTabs.StateDetails;

[QueryProperty(nameof(TimeRegistration), nameof(TimeRegistration))]
internal partial class StateDetailsViewModel : BaseViewModel
{
    [ObservableProperty]
    public TimeRegistration timeRegistration;
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
        if (timeRegistration.Accessibility == RegistrationStatus.Locked)
        {
            await Application.Current.MainPage.DisplayAlert("Registration locked", "Cannot edit a locked registration", "OK");
            return;
        }
        ShowAddButton = !ShowAddButton;
    }

    public void ShowEditStatePopup(ContentPage page)
    {
        AnalyticsService.Instance.TrackEvent(Event.Action, Category.Touch, "EditStatePopup tapped");
        var popup = new EditStatePopup(timeRegistration);
        page.ShowPopup(popup);
    }
}
