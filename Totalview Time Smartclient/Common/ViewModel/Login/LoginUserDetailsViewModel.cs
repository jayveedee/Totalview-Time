using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Totalview_Time_Smartclient.Common.ViewModel.Login;

public partial class LoginUserDetailsViewModel : BaseViewModel
{
    [ObservableProperty]
    private string userNameEntryValue;
    [ObservableProperty]
    private string passwordEntryValue;

    [ICommand]
    public async void LoginToServer()
    {
        throw new NotImplementedException();
    }

    [ICommand]
    public void ChangeServer()
    {
        throw new NotImplementedException();
    }
}
