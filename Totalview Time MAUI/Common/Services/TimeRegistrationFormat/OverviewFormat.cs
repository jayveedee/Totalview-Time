using Totalview_Time_MAUI.Common.Model.TimeManagement;

namespace Totalview_Time_MAUI.Common.Services.TimeRegistrationFormat;

internal record OverviewFormat
{
    public int Week { get; }
    public List<TimeRegistration> Registrations { get; }

    public OverviewFormat(int week, List<TimeRegistration> registrations)
    {
        Week = week;
        Registrations = registrations;
    }
}
