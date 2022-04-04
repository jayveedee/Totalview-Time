using System.Windows.Input;

namespace Totalview_Time_Smartclient.MVVM.ViewModel;

public class LoginServerDetailsViewModel : BindableObject
{
    public string ConnectToServerEntryValue { get; set; }
    public ICommand ConnectToServerCommand { get; set; }

    public LoginServerDetailsViewModel()
    {
        ConnectToServerCommand = new Command(ConnectToServer);
    }

    private void ConnectToServer()
    {
        throw new NotImplementedException();
    }
}
