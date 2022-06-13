using MvvmHelpers;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Totalview_Time_MAUI.Common.Model.TimeManagement;
using Totalview_Time_MAUI.Common.Services;
using Totalview_Time_MAUI.Common.Services.TimeRegistrationFormat;
using Totalview_Time_MAUI.Common.Services.TimeServerAPIService;
using Totalview_Time_MAUI.Common.Util;


namespace Totalview_Time_MAUI.Common.ViewModel.NavigationTabs;

internal partial class OverviewViewModel : BaseViewModel
{
    public ObservableCollection<TimeRegistration> TimeRegistrationsValue { get; set; }
    public bool IsRefreshingValue
    {
        get => isRefreshing;
        set => SetProperty(ref isRefreshing, value);
    }
    private bool isRefreshing;
    private List<TimeRegistration> timeRegistrations;

    public ICommand RefreshCommand { get; set; }
    

    public OverviewViewModel()
    {
        RefreshCommand = new Command(OnRefreshCommand);
        TimeRegistrationsValue = new ObservableRangeCollection<TimeRegistration>();
        var storage = StorageService.Instance.LoadStorage();
        timeRegistrations = DummyDataUtil.CreateTimeRegistrations();
        storage.TimeRegistrations = timeRegistrations;
        StorageService.Instance.SaveStorage(storage);
        OnRefreshCommand();
    }

    public void OnRefreshCommand()
    {
        IsRefreshingValue = true;
        //timeRegistrationsValue = await TimeServerAPIService.Instance.GetTimeRegistrations();

        TimeRegistrationsValue.Clear();
        for (int i = 0; i < timeRegistrations.Count; i++)
        {
            TimeRegistrationsValue.Add(timeRegistrations[i]);
        }
        IsRefreshingValue = false;
    }

    public async void ItemTappedCommand(object item)
    {
        if (item != null)
        {
            AnalyticsService.Instance.TrackEvent(Event.Action, Category.Touch, "OverviewRegistrationTapped");
            await Shell.Current.GoToAsync($"/{nameof(StateDetails)}", new Dictionary<string, object>
            {
                [nameof(TimeRegistration)] = (TimeRegistration) item,
                ["IsModifiedRegistration"] = false
            });
        }
    }
}
