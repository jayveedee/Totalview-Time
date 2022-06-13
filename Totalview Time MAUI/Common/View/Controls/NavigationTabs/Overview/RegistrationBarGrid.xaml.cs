using Totalview_Time_MAUI.Common.ViewModel.Controls.NavigationTabs.Overview;
using Totalview_Time_MAUI.Common.Model.TimeManagement;

namespace Totalview_Time_MAUI;

public partial class RegistrationBarGrid : Grid
{
	private readonly RegistrationBarGridViewmodel viewmodel;
	public RegistrationBarGrid()
	{
		InitializeComponent();
		viewmodel = new RegistrationBarGridViewmodel();
		BindingContext = viewmodel;
	}

	public static readonly BindableProperty RegistrationStatesProperty = BindableProperty.Create(
		nameof(RegistrationStates),
		typeof(List<TimeState>),
		typeof(RegistrationBarGrid),
		null,
		BindingMode.TwoWay);

	public object RegistrationStates
    {
		get => (List<TimeState>)GetValue(RegistrationStatesProperty);
		set => SetValue(RegistrationStatesProperty, value);
    }
}