using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Totalview_Time_MAUI.Common.Services;

namespace Totalview_Time_MAUI.Common.ViewModel.Login;

public partial class LoginUserDetailsViewModel : BaseViewModel
{
    [ObservableProperty]
    private string userNameEntryValue;
    [ObservableProperty]
    private string passwordEntryValue;

    [ICommand]
    public void LoginToServer()
    {
        throw new NotImplementedException();
    }

    [ICommand]
    public async void ChangeServer()
    {
        AnalyticsService.Instance.TrackEvent(Event.Action, Category.Touch, "ChangeServer tapped");

        await Shell.Current.GoToAsync($"//{nameof(LoginServerDetails)}");
    }
}
