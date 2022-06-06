namespace Totalview_Time_MAUI.Common.Model.TimeRegistration;

public record Registration
{
    public string CurrentDayTitle { get; }
    public string CurrentDateTitle { get; }
    public DateTime DateCreated { get; }
    public List<State> States { get; }
    public EditState Accessibility { get; }
    public string CompleteWorkHours { get; set; }
    public string NormalWorkHours { get; set; }
    public string OvertimeWorkHours { get; set; }
    public string VacationHours { get; set; }
    public string SicknessHours { get; set; }
    public string TimeOffWorkHours { get; set; }
    public string NoWorkHours { get; set; }

    public Registration(DateTime dateCreated, List<State> states, EditState accessibility)
    {
        CurrentDayTitle = dateCreated.ToString("dddd");
        CurrentDateTitle = dateCreated.ToString("d");
        DateCreated = dateCreated;
        States = states;
        Accessibility = accessibility;
        CalculateWorkedHours(dateCreated, states);
        
    }

    private void CalculateWorkedHours(DateTime day, List<State> states)
    {
        var normalWorkDate = day;
        var overtimeWorkDate = day;
        var vacationDate = day;
        var sicknessDate = day;
        var timeoffDate = day;
        var noWorkDate = day;
        for (int i = 0; i < states.Count; i++)
        {
            State state = states[i];
            switch (state.TimeAccount)
            {
                case TimeAccount.NormalWork:
                    normalWorkDate += (state.EndDate - state.StartDate);
                    break;
                case TimeAccount.OvertimeWork:
                    overtimeWorkDate += (state.EndDate - state.StartDate);
                    break;
                case TimeAccount.Vacation:
                    vacationDate += (state.EndDate - state.StartDate);
                    break;
                case TimeAccount.Sickness:
                    sicknessDate += (state.EndDate - state.StartDate);
                    break;
                case TimeAccount.TimeOffWork:
                    timeoffDate += (state.EndDate - state.StartDate);
                    break;
                case TimeAccount.NoWork:
                    noWorkDate += (state.EndDate - state.StartDate);
                    break;
            }
        }
        NormalWorkHours = normalWorkDate.ToString("HH:mm");
        OvertimeWorkHours = overtimeWorkDate.ToString("HH:mm");
        VacationHours = vacationDate.ToString("HH:mm");
        SicknessHours = sicknessDate.ToString("HH:mm");
        TimeOffWorkHours = timeoffDate.ToString("HH:mm");
        NoWorkHours = noWorkDate.ToString("HH:mm");
        CompleteWorkHours = (normalWorkDate.AddTicks(overtimeWorkDate.Ticks)).ToString("HH:mm");
    }
}

public enum EditState
{
    Open,
    Locked
}