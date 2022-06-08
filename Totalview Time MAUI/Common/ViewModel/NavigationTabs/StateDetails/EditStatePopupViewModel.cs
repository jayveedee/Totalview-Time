using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Totalview_Time_MAUI.Common.Model.TimeManagement;
using Totalview_Time_MAUI.Common.Services;
using Totalview_Time_MAUI.Common.Util;

namespace Totalview_Time_MAUI.Common.ViewModel.NavigationTabs.StateDetails;

internal partial class EditStatePopupViewModel : BaseViewModel
{
    [ObservableProperty]
    public List<TimeState> states;
    [ObservableProperty]
    public TimeState selectedState;
    [ObservableProperty]
    public TimeSpan startTime;
    [ObservableProperty]
    public TimeSpan endTime;
    [ObservableProperty]
    public string descriptionText;
    [ObservableProperty]
    public string extraText;
    private readonly Popup popup;
    private readonly TimeRegistration registration;
    private readonly TimeSpan defaultStartTime;
    private readonly TimeSpan defaultEndTime;

    public EditStatePopupViewModel()
    {
        defaultStartTime = startTime;
        defaultEndTime = endTime;
    }

    public EditStatePopupViewModel(TimeRegistration registration, EditStatePopup popup)
    {
        this.registration = registration;
        this.popup = popup;
        states = DummyDataUtil.CreateStateList();
        defaultStartTime = startTime;
        defaultEndTime = endTime;
    }

    [ICommand]
    public async void Save()
    {
        if (await ValidateCreatingState())
        {
            bool result = await DisplayCautionAlert("Creating state", "You are about to create the state, are you sure everything is in order?", true);
            if (result)
            {
                AnalyticsService.Instance.TrackEvent(Event.Action, Category.Touch, "SaveNewlyCreatedState");
                popup.Close();
            }
        }
    }

    [ICommand]
    public async void Delete()
    {
        if (ValidateDeletingState())
        {
            bool result = await DisplayCautionAlert("Creating state in progress", "Are you sure you want to delete this new state?", true);
            if (!result)
            {
                return;
            }
        }

        AnalyticsService.Instance.TrackEvent(Event.Action, Category.Touch, "DeleteNewlyCreatedState");
        popup.Close();
    }

    private async Task<bool> DisplayCautionAlert(string title, string description, bool hasMultiChoice = false)
    {
        if (hasMultiChoice)
        {
            bool result = await Application.Current.MainPage.DisplayAlert(title, description, "Yes", "No");
            return result;
        } 
        else
        {
            await Application.Current.MainPage.DisplayAlert(title, description, "OK");
            return true;
        }
    }

    private bool ValidateDeletingState()
    {
        if (selectedState != null)
        {
            return true;
        }
        else if (startTime.TotalMilliseconds != defaultStartTime.TotalMilliseconds)
        {
            return true;
        }
        else if (endTime.TotalMilliseconds != defaultEndTime.TotalMilliseconds)
        {
            return true;
        }
        else if (!string.IsNullOrEmpty(descriptionText))
        {
            return true;
        }
        else if (!string.IsNullOrEmpty(extraText))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private async Task<bool> ValidateCreatingState()
    {
        if (selectedState == null)
        {
            await DisplayCautionAlert("Selected state is invalid", "Please select a valid state. A state must be selected to continue.");
            return false;
        }
        else if ((startTime > endTime || startTime == endTime) && !(startTime.TotalMilliseconds != 0 && endTime.TotalMilliseconds == 0))
        {
            await DisplayCautionAlert("Start time is invalid", "Please enter a valid start time. Start time cannot start after end time or start and end at the same time.");
            return false;
        }
        else if (string.IsNullOrEmpty(descriptionText))
        {
            await DisplayCautionAlert("Description field missing", "Please enter a valid description. Description must not be empty.");
            return false;
        }

        bool multipleStates = false;
        List<TimeState> overlappingStates = new List<TimeState>();
        List<TimeState> modifiedStates = registration.States;
        for (int i = 0; i < registration.States.Count; i++)
        {
            var registeredState = registration.States[i];
            var modifiedState = modifiedStates[i];

            var registeredStateStartTime = registeredState.StartDate.TimeOfDay;
            var registeredStateEndTime = registeredState.EndDate.TimeOfDay;
            var modifiedStateStartTime = modifiedState.StartDate.TimeOfDay;
            var modifiedStateEndTime = modifiedState.EndDate.TimeOfDay;

            // Newly created state starts in or exactly when the current state starts
            if ((startTime >= registeredStateStartTime && (startTime < registeredStateEndTime || registeredStateEndTime.TotalMilliseconds == 0)) 
                || (startTime < registeredStateStartTime && (startTime < registeredStateEndTime || registeredStateEndTime.TotalMilliseconds == 0)))
            {
                // Newly created state ends after the current state end
                if (endTime > registeredStateEndTime && registeredStateEndTime.TotalMilliseconds != 0)
                {
                    overlappingStates.Add(registeredState);
                    multipleStates = true;
                }
                // Newly created state's end is within the current state's endtime
                else if ((endTime <= registeredStateEndTime || registeredStateEndTime.TotalMilliseconds == 0) && (endTime > registeredStateStartTime || endTime.TotalMilliseconds == 0))
                {
                    // There were multiple state overlaps
                    if (multipleStates)
                    {
                        overlappingStates.Add(registeredState);
                    }
                    // Only a single state overlap was found
                    else
                    {
                        var stateCautionText = $"The state [{registeredState.Title}, {registeredState.StartTime} - {registeredState.EndTime}] needs to be modified\n";
                        await DisplayCautionAlert("State overlap", $"{stateCautionText}");
                        overlappingStates.Add(registeredState);

                        

                        break;
                    }
                }
                // The last state in the registration was also overlapping with the newly created state
                else if (multipleStates)
                {
                    break;
                }
            }
        }
        if (multipleStates)
        {
            string extraStateCautionText = "";
            for (int j = 0; j < overlappingStates.Count; j++)
            {
                var extraState = overlappingStates[j];
                extraStateCautionText += $"The state [{extraState.Title}, {extraState.StartTime} - {extraState.EndTime}] needs to be modified\n";
            }
            await DisplayCautionAlert("State overlap", $"{extraStateCautionText}");
        }

        if (overlappingStates.Count > 0)
        {
            bool result = await DisplayCautionAlert("State creation action", "You are about to replace the overlapping states with the new state, are you sure you want to proceed?", true);
            if (!result)
            {
                return false;
            }
            
        }

        return true;
    }
}
