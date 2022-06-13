using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;
using System.Collections.ObjectModel;
using Totalview_Time_MAUI.Common.Model.TimeManagement;
using Totalview_Time_MAUI.Common.Services;
using Totalview_Time_MAUI.Common.Util;

namespace Totalview_Time_MAUI.Common.ViewModel.NavigationTabs;

internal partial class TimeAccountsViewModel : BaseViewModel
{
    public ObservableRangeCollection<TimeAccount> TimeAccounts { get; set; }

    public TimeAccountsViewModel()
    {
        TimeAccounts = new ObservableRangeCollection<TimeAccount>();
        TimeAccounts.AddRange(DummyDataUtil.CreateTimeAccountList());
    }

    [ICommand]
    public void ItemTapped(TimeAccount account)
    {
        AnalyticsService.Instance.TrackEvent(Event.Action, Category.Touch, "TimeAccountTapped");
    }
}
