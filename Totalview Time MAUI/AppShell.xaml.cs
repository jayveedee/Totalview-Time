using Totalview_Time_MAUI.Common.ViewModel;

namespace Totalview_Time_MAUI;

public partial class AppShell : Shell
{
    private readonly AppShellViewModel viewModel;
	public AppShell()
	{
		InitializeComponent();

        viewModel = new AppShellViewModel();

        AddRoutes();
    }

    protected override void OnAppearing()
    {
        viewModel.DetermineLoginRoute();
    }

    private void AddRoutes()
    {
        Routing.RegisterRoute(nameof(LoginServerDetails), typeof(LoginServerDetails));
        Routing.RegisterRoute(nameof(LoginUserDetails), typeof(LoginUserDetails));
        Routing.RegisterRoute(nameof(TimeAccounts), typeof(TimeAccounts));
        Routing.RegisterRoute(nameof(Overview), typeof(Overview));
        Routing.RegisterRoute(nameof(Inquiry), typeof(Inquiry));
        Routing.RegisterRoute(nameof(StateDetails), typeof(StateDetails));
    }
}