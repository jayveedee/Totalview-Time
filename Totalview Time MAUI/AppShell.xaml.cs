using Totalview_Time_Smartclient.Common.ViewModel;

namespace Totalview_Time_Smartclient;

public partial class AppShell : Shell
{
    private readonly AppShellViewModel viewModel;
	public AppShell()
	{
		InitializeComponent();

        viewModel = new AppShellViewModel();
    }

    protected override void OnAppearing()
    {
        viewModel.DetermineLoginRoute();
    }
}