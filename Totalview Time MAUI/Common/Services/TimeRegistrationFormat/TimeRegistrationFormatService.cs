using System.Globalization;
using Totalview_Time_MAUI.Common.Model.TimeManagement;

namespace Totalview_Time_MAUI.Common.Services.TimeRegistrationFormat;

internal interface ITimeRegistrationFormatService
{
    List<OverviewFormat> SortRegistrationsByWeekNumber(List<TimeRegistration> unsortedList);
}

internal class TimeRegistrationFormatService : ITimeRegistrationFormatService
{
    public List<OverviewFormat> SortRegistrationsByWeekNumber(List<TimeRegistration> unsortedList)
    {
        var sortedList = new List<OverviewFormat>();
        for (int i = 0; i < unsortedList.Count; i++)
        {
            var registration = unsortedList[i];
            var weekNumber = ISOWeek.GetWeekOfYear(registration.DateCreated);
            var listOfRegistrationsInWeek = new List<TimeRegistration>();

            for (int j = 0; j < unsortedList.Count; j++)
            {
                var otherRegistration = unsortedList[j];
                var otherWeekNumber = ISOWeek.GetWeekOfYear(otherRegistration.DateCreated);
                if (weekNumber == otherWeekNumber)
                {
                    listOfRegistrationsInWeek.Add(otherRegistration);
                    unsortedList.RemoveAt(j);
                    j--;
                } 
                else
                {
                    break;
                }
            }
            sortedList.Add(new OverviewFormat(weekNumber, listOfRegistrationsInWeek));
            
        }
        return sortedList;
    }
}
