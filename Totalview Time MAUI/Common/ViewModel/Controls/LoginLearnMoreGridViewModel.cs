using CommunityToolkit.Mvvm.Input;

namespace Totalview_Time_MAUI.Common.ViewModel.Controls;

public partial class LoginLearnMoreGridViewModel : BaseViewModel
{
    private const string url = "https://www.total-view.com/totalview-suite/#tv_time_section";

    [ICommand]
    public async void LearnMore()
    {
        await Browser.OpenAsync(url, new BrowserLaunchOptions
        {
            LaunchMode = BrowserLaunchMode.SystemPreferred,
            TitleMode = BrowserTitleMode.Show,
            PreferredToolbarColor = Colors.AliceBlue,
            PreferredControlColor = Colors.Violet
        });
    }
}
