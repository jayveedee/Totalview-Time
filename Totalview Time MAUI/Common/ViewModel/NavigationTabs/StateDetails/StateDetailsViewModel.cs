using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;
using Totalview_Time_MAUI.Common.Model.TimeManagement;
using Totalview_Time_MAUI.Common.Services;
using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;

namespace Totalview_Time_MAUI.Common.ViewModel.NavigationTabs.StateDetails;

[QueryProperty(nameof(TimeRegistration), nameof(TimeRegistration))]
[QueryProperty("IsModifiedRegistration", nameof(IsModifiedRegistration))]
internal partial class StateDetailsViewModel : BaseViewModel
{
    [ObservableProperty]
    public TimeRegistration timeRegistration;
    public bool IsModifiedRegistration;
    public ObservableRangeCollection<TimeState> States { get; set; }
    public string CompleteWorkHours { 
        get => completeWorkHours; 
        set => SetProperty(ref completeWorkHours, value); 
    }
    private string completeWorkHours;
    public string NormalWorkHours
    {
        get => normalWorkHours;
        set => SetProperty(ref normalWorkHours, value);
    }
    private string normalWorkHours;
    public string OvertimeWorkHours
    {
        get => overtimeWorkHours;
        set => SetProperty(ref overtimeWorkHours, value);
    }
    private string overtimeWorkHours;
    public string VacationHours
    {
        get => vacationHours;
        set => SetProperty(ref vacationHours, value);
    }
    private string vacationHours;

    public bool ToolbarItemVisibility { 
        get => toolbarItemVisibiliy;
        set => SetProperty(ref toolbarItemVisibiliy, value); 
    }
    private bool toolbarItemVisibiliy;
    private bool registrationIsEdited;

    public void ItemTappedCommand(object item)
    {
        if (item != null)
        {
            AnalyticsService.Instance.TrackEvent(Event.Action, Category.Touch, "StateDetailsStateTapped");
        }
    }

    public bool EditRegistrationVisibility {
        get => editRegistrationVisibility;
        set => SetProperty(ref editRegistrationVisibility, value);
    }
    private bool editRegistrationVisibility;
    public bool ShowAddButton
    {
        get => showAddButton;
        set => SetProperty(ref showAddButton, value);
    }
    private bool showAddButton;

    public StateDetailsViewModel()
    {
        ShowAddButton = false;
        ToolbarItemVisibility = false;
        EditRegistrationVisibility = true;
        registrationIsEdited = false;
        States = new ObservableRangeCollection<TimeState>();
    }

    [ICommand]
    public async void EditRegistration()
    {
        AnalyticsService.Instance.TrackEvent(Event.Action, Category.Touch, "EditRegistration tapped");
        if (timeRegistration.Accessibility == RegistrationStatus.Locked)
        {
            await Application.Current.MainPage.DisplayAlert("Registration locked", "Cannot edit a locked registration", "OK");
            return;
        }
        ShowAddButton = !ShowAddButton;
        ToolbarItemVisibility = true;
        EditRegistrationVisibility = false;
    }

    [ICommand]
    public async void Save()
    {
        if (!registrationIsEdited)
        {
            await Shell.Current.GoToAsync("..");
            return;
        }
        var storage = StorageService.Instance.LoadStorage();

        if (storage.PendingTimeRegistrations == null)
        {
            storage.PendingTimeRegistrations = new List<TimeRegistration>();
        }

        timeRegistration.InquiryStatus = RegistrationInquiryStatus.Pending;
        storage.PendingTimeRegistrations.Add(timeRegistration);

        StorageService.Instance.SaveStorage(storage);

        AnalyticsService.Instance.TrackEvent(Event.Action, Category.Touch, "SaveNewlyCreatedStateTapped");
        await Shell.Current.GoToAsync("..");
    }

    public void UpdateList()
    {
        States.Clear();
        States.AddRange(timeRegistration.States);
        CompleteWorkHours = timeRegistration.CompleteWorkHours;
        NormalWorkHours = timeRegistration.NormalWorkHours;
        OvertimeWorkHours = timeRegistration.OvertimeWorkHours;
        VacationHours = timeRegistration.VacationHours;
    }

    public async void ShowEditStatePopup(ContentPage page)
    {
        AnalyticsService.Instance.TrackEvent(Event.Action, Category.Touch, "EditStatePopup tapped");
        var popup = new EditStatePopup(timeRegistration);
        var result = await page.ShowPopupAsync(popup);

        if (result != null)
        {
            timeRegistration.UpdateStates((List<TimeState>)result);
            CompleteWorkHours = timeRegistration.CompleteWorkHours;
            NormalWorkHours = timeRegistration.NormalWorkHours;
            OvertimeWorkHours = timeRegistration.OvertimeWorkHours;
            VacationHours = timeRegistration.VacationHours;
            States.Clear();
            States.AddRange(timeRegistration.States);
            registrationIsEdited = true;
        }
    }
}
