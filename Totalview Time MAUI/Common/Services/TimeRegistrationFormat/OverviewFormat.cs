using Totalview_Time_MAUI.Common.Model.TimeRegistration;

namespace Totalview_Time_MAUI.Common.Services.TimeRegistrationFormat;

internal record OverviewFormat
{
    public int Week { get; }
    public List<Registration> Registrations { get; }

    public OverviewFormat(int week, List<Registration> registrations)
    {
        Week = week;
        Registrations = registrations;
    }
}
