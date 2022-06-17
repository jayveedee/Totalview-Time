namespace Totalview_Time_MAUI.Common.Model.TimeManagement;

public record class TimeState
{
    public string Title { get; }
    public string Description { get; }
    public string Extra { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public Color Color { get; }
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }
    public TimeAccountType TimeAccountType { get; }
    public bool IsModified { get; set;  }
    public TimeState OldState { get; set;  }
    public EditStatus EditStatus { get; set; }

    public TimeState(string title, string description, string extra, Color color, DateTime startDate, DateTime endDate, TimeAccountType timeAccountType)
    {
        Title = title;
        Description = description;
        Extra = extra;
        Color = color;
        StartDate = startDate;
        EndDate = endDate;
        TimeAccountType = timeAccountType;
        StartTime = startDate.ToString("HH:mm");
        EndTime = endDate.ToString("HH:mm");
        EditStatus = EditStatus.NoChange;
    }
}

public enum TimeAccountType
{
    NormalWork,
    OvertimeWork,
    Vacation,
    Sickness,
    TimeOffWork,
    NoWork
}

public enum EditStatus
{
    NoChange,
    PartialChange,
    Removed
}
