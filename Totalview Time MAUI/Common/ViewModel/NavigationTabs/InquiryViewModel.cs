using CommunityToolkit.Mvvm.Input;
using Totalview_Time_MAUI.Common.Model.TimeManagement;
using Totalview_Time_MAUI.Common.Services;

namespace Totalview_Time_MAUI.Common.ViewModel.NavigationTabs;

internal partial class InquiryViewModel : BaseViewModel
{
    public InquiryViewModel()
    {

    }

    [ICommand]
    public void AcceptedButton()
    {
        AnalyticsService.Instance.TrackEvent(Event.Action, Category.Touch, "InquiryAcceptedButtonTapped");
    }

    [ICommand]
    public void RejectedButton()
    {
        AnalyticsService.Instance.TrackEvent(Event.Action, Category.Touch, "InquiryRejectedButtonTapped");
    }

    [ICommand]
    public async void InProgressButton()
    {
        AnalyticsService.Instance.TrackEvent(Event.Action, Category.Touch, "InquiryInProgressButtonTapped");
        await Shell.Current.GoToAsync($"/{nameof(InProgress)}", new Dictionary<string, object>
        {
            ["TimeRegistrations"] = StorageService.Instance.LoadStorage().PendingTimeRegistrations
        });
    }

    [ICommand]
    public void DraftButton()
    {
        AnalyticsService.Instance.TrackEvent(Event.Action, Category.Touch, "InquiryDraftButtonTapped");
    }
}
