namespace Totalview_Time_MAUI.Common.Model.TimeManagement;

public class TimeAccount
{
    public string Title { get; set; }
    public int Hours { get; set; }
    public List<TimeRegistration> Registrations { get; set; }
    public Color Color { get; set; }

    public TimeAccount(string title, int hours, List<TimeRegistration> registrations, Color color)
    {
        Title = title;
        Hours = hours;
        Registrations = registrations;
        Color = color;
    }
}
