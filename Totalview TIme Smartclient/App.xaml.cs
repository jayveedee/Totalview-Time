namespace Totalview_Time_Smartclient;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        //MainPage = new MainPage();
        //MainPage = new LoginServerDetails();
        MainPage = new LoginUserDetails();
    }
}
