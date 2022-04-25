using Totalview_Time_MAUI.Common.ViewModel;

namespace Totalview_Time_MAUI;

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