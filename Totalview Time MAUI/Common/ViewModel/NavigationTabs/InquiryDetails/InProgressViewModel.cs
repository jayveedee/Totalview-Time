using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;
using System.Collections.ObjectModel;
using Totalview_Time_MAUI.Common.Model.TimeManagement;
using Totalview_Time_MAUI.Common.Services;

namespace Totalview_Time_MAUI.Common.ViewModel.NavigationTabs.InquiryDetails;

[QueryProperty("TimeRegistrations", nameof(TimeRegistrations))]
internal partial class InProgressViewModel : BaseViewModel
{
    public ObservableRangeCollection<TimeRegistration> TimeRegistrationsValue { get; set; }
    public List<TimeRegistration> TimeRegistrations { get; set; }
    public bool IsRefreshingValue
    {
        get => isRefreshing;
        set => SetProperty(ref isRefreshing, value);
    }
    private bool isRefreshing;

    public InProgressViewModel()
    {
        TimeRegistrationsValue = new ObservableRangeCollection<TimeRegistration>();
        Refresh();
    }

    [ICommand]
    public void Refresh()
    {
        IsRefreshingValue = true;

        TimeRegistrationsValue.Clear();

        if (TimeRegistrations != null)
        {
            TimeRegistrationsValue.AddRange(TimeRegistrations);
        }

        IsRefreshingValue = false;
    }

    public async void ItemTappedCommand(object item)
    {
        if (item != null)
        {
            AnalyticsService.Instance.TrackEvent(Event.Action, Category.Touch, "PendingRegistrationTapped");
            await Shell.Current.GoToAsync($"/{nameof(StateDetails)}", new Dictionary<string, object>
            {
                [nameof(TimeRegistration)] = (TimeRegistration)item,
                ["IsModifiedRegistration"] = true
            });
        }
    }

    public void UpdateList()
    {
        TimeRegistrationsValue.Clear();
        TimeRegistrationsValue.AddRange(TimeRegistrations);
    }
}
