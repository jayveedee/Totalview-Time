namespace Totalview_Time_MAUI.Common.Model.TimeRegistration;

public record class State
{
    public string Title { get; }
    public string Description { get; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public Color Color { get; }
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }
    public TimeAccount TimeAccount { get; }
    public bool IsModified { get; set;  }
    public State ModifiedState { get; set;  }
    public List<State> Branches { get; set;  }

    public State(string title, string description, Color color, DateTime startDate, DateTime endDate, TimeAccount timeAccount)
    {
        Title = title;
        Description = description;
        Color = color;
        StartDate = startDate;
        EndDate = endDate;
        TimeAccount = timeAccount;
        StartTime = startDate.ToString("HH:mm");
        EndTime = endDate.ToString("HH:mm");
    }

    public void ModifyState(State modifiedState, List<State> branches)
    {
        IsModified = true;
        ModifiedState = modifiedState;
        Branches = branches;
    }
}

public enum TimeAccount
{
    NormalWork,
    OvertimeWork,
    Vacation,
    Sickness,
    TimeOffWork,
    NoWork
}
