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
    private List<TimeState> newRegistrationStates;
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
                popup.Close(newRegistrationStates);
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
        popup.Close(null);
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

        newRegistrationStates = await ModifyStates(registration.States);
        return true;
    }

    private async Task<List<TimeState>> ModifyStates(List<TimeState> originalStates)
    {
        var modifiedStates = new List<TimeState>();
        var overlappingStates = new List<TimeState>();

        var isFirstOverlap = true;
        for (int i = 0; i < originalStates.Count; i++)
        {
            var orState = originalStates[i];
            var orStateStartTime = orState.StartDate.TimeOfDay;
            var orStateEndTime = orState.EndDate.TimeOfDay;

            if (!IsCurrentStateOverlapping(orStateStartTime, orStateEndTime))
            {
                modifiedStates.Add(orState);
                continue;
            }

            modifiedStates.AddRange(CreateModifiedStates(orState, isFirstOverlap, isFirstOverlap, false));
            isFirstOverlap = false;

            overlappingStates.Add(orState);

            if (IsNewStateWithinCurrentState(orStateEndTime))
            {
                modifiedStates.AddRange(CreateModifiedStates(orState, isFirstOverlap, isFirstOverlap, true));

                for (int j = i + 1; j < originalStates.Count; j++)
                {
                    modifiedStates.Add(originalStates[j]);
                }

                break;
            }
        }

        await HandleDisplayingOverlapAlert(overlappingStates);

        return modifiedStates;
    }

    private bool IsCurrentStateOverlapping(TimeSpan currStartTime, TimeSpan currEndTime)
    {
        return ((startTime >= currStartTime && (startTime < currEndTime || currEndTime.TotalMilliseconds == 0))
                || (startTime < currStartTime && (startTime < currEndTime || currEndTime.TotalMilliseconds == 0)));
    }

    private bool IsNewStateWithinCurrentState(TimeSpan currEndTime)
    {
        return !(endTime > currEndTime && currEndTime.TotalMilliseconds != 0 && endTime != currEndTime);   
    }

    private List<TimeState> CreateModifiedStates(TimeState currState, bool start, bool middle, bool end)
    {
        var modifiedStates = new List<TimeState>();

        var currStartTime = currState.StartDate.TimeOfDay;
        var currEndTime = currState.EndDate.TimeOfDay;

        var startConstant = new TimeSpan();
        var endConstant = new TimeSpan();

        if (start && currStartTime.TotalMilliseconds != startTime.TotalMilliseconds)
        {
            if (currStartTime.TotalMilliseconds == 0 || startTime > currStartTime)
            {

                endConstant = startTime > currEndTime ? startTime - currEndTime : currEndTime - startTime;
                var startDate = currState.StartDate;

                if (currStartTime.TotalMilliseconds == 0 || startTime > currStartTime)
                {
                    startDate = currState.StartDate;
                }
                else if (currStartTime <= startTime)
                {
                    startConstant = endTime >= currStartTime ? endTime - currStartTime : currStartTime - endTime;
                    startDate = currState.StartDate.Add(startConstant);
                }

                var endDate = currState.EndDate;

                if (currEndTime.TotalMilliseconds != 0)
                {
                    endDate = currState.EndDate.Subtract(endConstant);
                }
                else
                {
                    endDate = currState.StartDate.Add(endConstant - currStartTime);
                }

                modifiedStates.Add(new TimeState(
                    currState.Title,
                    currState.Description,
                    currState.Extra,
                    currState.Color,
                    startDate,
                    endDate,
                    currState.TimeAccountType));
            }
        }
        if (middle)
        {
            startConstant = startTime > currStartTime ? startTime - currStartTime : currStartTime - startTime;

            endConstant = currEndTime > endTime ? endTime - currEndTime : currEndTime - endTime;

            var endDate = currState.EndDate;

            if (currEndTime > endTime)
            {
                endDate = currState.EndDate.Add(endConstant);
            } 
            else
            {
                endDate = currState.EndDate.Subtract(endConstant);
            }
            
            modifiedStates.Add(new TimeState(
                selectedState.Title,
                DescriptionText,
                ExtraText,
                selectedState.Color,
                currState.StartDate.Add(startConstant),
                endDate,
                selectedState.TimeAccountType));
        }
        if (end && (currEndTime > endTime || currEndTime.TotalMilliseconds == 0))
        {
            startConstant = endTime > currEndTime ? endTime - currEndTime : currEndTime - endTime;
            var startDate = currState.EndDate.Subtract(startConstant);

            if (currEndTime.TotalMilliseconds == 0)
            {
                startDate = currState.StartDate.Add(endTime - currState.StartDate.TimeOfDay);
            }

            modifiedStates.Add(new TimeState(
                currState.Title,
                currState.Description,
                currState.Extra,
                currState.Color,
                startDate,
                currState.EndDate,
                currState.TimeAccountType));
        }

        return modifiedStates;
    }

    private async Task<bool> HandleDisplayingOverlapAlert(List<TimeState> overlappingStates)
    {
        var stateCautionText = "";
        foreach (var overlapState in overlappingStates)
        {
            stateCautionText += $"The state [{overlapState.Title}, {overlapState.StartTime} - {overlapState.EndTime}] needs to be modified\n";
        }

        return await DisplayCautionAlert("State overlap", $"{stateCautionText}");
    }
}
