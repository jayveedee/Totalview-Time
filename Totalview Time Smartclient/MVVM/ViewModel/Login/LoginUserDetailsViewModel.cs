using System.Windows.Input;

namespace Totalview_Time_Smartclient.MVVM.ViewModel.Login;

public class LoginUserDetailsViewModel : BaseViewModel
{
    public string UserNameEntryValue { get; set; }
    public string PasswordEntryValue { get; set; }
    public ICommand LoginToServerCommand { get; set; }
    public ICommand ChangeServerCommand { get; set; }
    public LoginUserDetailsViewModel() : base()
    {
        LoginToServerCommand = new Command(LoginToServer);
        ChangeServerCommand = new Command(ChangeServer);
    }

    public async void LoginToServer()
    {
        throw new NotImplementedException();
    }

    public void ChangeServer()
    {
        throw new NotImplementedException();
    }
}
