using CommunityToolkit.Mvvm.ComponentModel;
using MvvmHelpers;
using System.Collections.ObjectModel;
using Totalview_Time_MAUI.Common.Model.TimeManagement;
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
}
