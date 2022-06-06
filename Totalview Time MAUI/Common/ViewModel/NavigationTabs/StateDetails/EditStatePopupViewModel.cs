using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Totalview_Time_MAUI.Common.Model.TimeRegistration;
using Totalview_Time_MAUI.Common.Services;
using Totalview_Time_MAUI.Common.Util;

namespace Totalview_Time_MAUI.Common.ViewModel.NavigationTabs.StateDetails;

internal partial class EditStatePopupViewModel : BaseViewModel
{
    [ObservableProperty]
    public List<State> states;
    [ObservableProperty]
    public State selectedState;
    [ObservableProperty]
    public TimeSpan startTime;
    [ObservableProperty]
    public TimeSpan endTime;
    [ObservableProperty]
    public string descriptionText;
    [ObservableProperty]
    public string extraText;
    private Popup popup;
    private Registration registration;
    private TimeSpan defaultStartTime;
    private TimeSpan defaultEndTime;

    public EditStatePopupViewModel()
    {
        defaultStartTime = startTime;
        defaultEndTime = endTime;
    }

    public EditStatePopupViewModel(Registration registration, EditStatePopup popup)
    {
        this.registration = registration;
        states = DummyDataUtil.CreateStateList();
        this.popup = popup;
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
        List<State> extraStates = new List<State>();
        for (int i = 0; i < registration.States.Count; i++)
        {
            var registeredState = registration.States[i];

            var registeredStateStartTime = registeredState.StartDate.TimeOfDay;
            var registeredStateEndTime = registeredState.EndDate.TimeOfDay;

            if ((startTime >= registeredStateStartTime && (startTime < registeredStateEndTime || registeredStateEndTime.TotalMilliseconds == 0)) 
                || (startTime < registeredStateStartTime && (startTime < registeredStateEndTime || registeredStateEndTime.TotalMilliseconds == 0)))
            {
                if (endTime > registeredStateEndTime && registeredStateEndTime.TotalMilliseconds != 0)
                {
                    extraStates.Add(registeredState);
                    multipleStates = true;
                }
                else if ((endTime <= registeredStateEndTime || registeredStateEndTime.TotalMilliseconds == 0) && (endTime > registeredStateStartTime || endTime.TotalMilliseconds == 0))
                {
                    if (multipleStates)
                    {
                        extraStates.Add(registeredState);
                    }
                    else
                    {
                        var stateCautionText = $"The state [{registeredState.Title}, {registeredState.StartTime} - {registeredState.EndTime}] needs to be modified\n";
                        await DisplayCautionAlert("State overlap", $"{stateCautionText}");
                        break;
                    }
                }
                else if (multipleStates)
                {
                    break;
                }
            }
        }
        if (multipleStates)
        {
            string extraStateCautionText = "";
            for (int j = 0; j < extraStates.Count; j++)
            {
                var extraState = extraStates[j];
                extraStateCautionText += $"The state [{extraState.Title}, {extraState.StartTime} - {extraState.EndTime}] needs to be modified\n";
            }
            await DisplayCautionAlert("State overlap", $"{extraStateCautionText}");
        }

        return true;
    }
}
