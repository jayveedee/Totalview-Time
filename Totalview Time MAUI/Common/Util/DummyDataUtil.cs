using Totalview_Time_MAUI.Common.Model.TimeManagement;

namespace Totalview_Time_MAUI.Common.Util;

internal static class DummyDataUtil
{
    private readonly static List<string> _listOfTitles;
    private readonly static List<string> _listOfDescriptions;
    private readonly static List<Color> _listOfColors;

    static DummyDataUtil()
    {
        _listOfTitles = new List<string>();
        _listOfDescriptions = new List<string>();
        _listOfColors = new List<Color>();

        _listOfTitles.Add("Inactive");
        _listOfTitles.Add("In");
        _listOfTitles.Add("Busy");
        _listOfTitles.Add("Lunch");
        _listOfTitles.Add("Home");

        _listOfDescriptions.Add("No state set");
        _listOfDescriptions.Add("At the office");
        _listOfDescriptions.Add("In a meeting");
        _listOfDescriptions.Add("At the cantime");
        _listOfDescriptions.Add("Signed off for the day");

        _listOfColors.Add(Colors.Red);
        _listOfColors.Add(Colors.Green);
        _listOfColors.Add(Colors.Blue);
        _listOfColors.Add(Colors.Brown);
        _listOfColors.Add(Colors.Black);
    }

    public static List<TimeRegistration> CreateTimeRegistrations(int amount = 15)
    {
        var random = new Random();
        var listOfRegistrations = new List<TimeRegistration>();

        var enumValues = Enum.GetValues(typeof(RegistrationStatus));
        for (int i = 0; i < amount; i++)
        {
            var day = DateTime.Today.AddDays(-i + 1);
            var registration = new TimeRegistration(day, CreateTimeStates(day), (RegistrationStatus)enumValues.GetValue(random.Next(enumValues.Length)));
            listOfRegistrations.Add(registration);
        }
        return listOfRegistrations;
    }

    public static List<TimeAccount> CreateTimeAccountList()
    {
        var random = new Random();
        var listOfAccounts = new List<TimeAccount>();
        var timeRegistrations = CreateTimeRegistrations(15);
        for (int i = 0; i < Enum.GetNames(typeof(TimeAccountType)).Length - 1; i++)
        {
            string title = TimeAccountType.NormalWork.ToString();
            TimeAccountType currentAccountType = TimeAccountType.NormalWork;
            Color color = Color.FromArgb("00CF6C");
            if (i == 1)
            {
                title = TimeAccountType.OvertimeWork.ToString();
                currentAccountType = TimeAccountType.OvertimeWork;
                color = Color.FromArgb("00D8CA");
            }
            else if (i == 2)
            {
                title = TimeAccountType.Vacation.ToString();
                currentAccountType = TimeAccountType.Vacation;
                color = Color.FromArgb("A920CB");
            }
            else if (i == 3)
            {
                title = TimeAccountType.Sickness.ToString();
                currentAccountType = TimeAccountType.Sickness;
                color = Color.FromArgb("EB7100");
            }
            else if (i == 4)
            {
                title = TimeAccountType.TimeOffWork.ToString();
                currentAccountType = TimeAccountType.TimeOffWork;
                color = Color.FromArgb("79697D");
            }
            else if (i == 5)
            {
                title = TimeAccountType.NoWork.ToString();
                currentAccountType = TimeAccountType.NoWork;
                color = Color.FromArgb("000000");
            }

            double hours = 0;
            for (int j = 0; j < timeRegistrations.Count; j++)
            {
                var timeStates = timeRegistrations[j].States;
                for (int k = 0; k < timeStates.Count; k++)
                {
                    var timeState = timeStates[k];
                    if (timeState.TimeAccountType == currentAccountType)
                    {
                        hours += (timeState.EndDate - timeState.StartDate).TotalHours;
                    }
                }
            }
            var timeAccount = new TimeAccount(title, Convert.ToInt32(hours), timeRegistrations, color);
            listOfAccounts.Add(timeAccount);
        }

        return listOfAccounts;
    }

    public static TimeRegistration CreateTimeRegistration()
    {
        var yesterday = DateTime.Today.AddDays(-1);
        var registration = new TimeRegistration(yesterday, CreateTimeStates(yesterday), RegistrationStatus.Open);
        return registration;
    }

    public static List<TimeState> CreateStateList()
    {
        var states = new List<TimeState>();
        for (int i = 0; i < _listOfTitles.Count; i++)
        {
            var timeAccountType = TimeAccountType.NoWork;

            if (i == 1 || i == 2)
            {
                timeAccountType = TimeAccountType.NormalWork;
            }

            var state = new TimeState(_listOfTitles[i], _listOfDescriptions[i], "test", _listOfColors[i], DateTime.Now, DateTime.Now, timeAccountType);
            states.Add(state);
        }
        states.Sort((x, y) => x.Title.CompareTo(y.Title));
        return states;
    }

    public static List<TimeState> CreateTimeStates(DateTime day, int amount = 5)
    {
        var random = new Random();
        var listOfStates = new List<TimeState>();

        var start = day;
        var end = DateTime.Today.AddHours(8);
        for (int i = 0; i < amount; i++)
        {
            var title = _listOfTitles[i];
            var description = _listOfDescriptions[i];
            var color = _listOfColors[i];
            var timeAccount = TimeAccountType.NoWork;
            
            if (i == 1)
            {
                start = end;
                end = start.AddMinutes(random.Next(1, 4) * 30);
                timeAccount = TimeAccountType.NormalWork;
            }
            else if (i == 2)
            {
                start = end;
                end = DateTime.Today.AddHours(12);
                timeAccount = TimeAccountType.NormalWork;
            }
            else if (i == 3)
            {
                start = end;
                var breakTime = random.Next(1, 2) * 30;
                end = start.AddMinutes(breakTime);
                timeAccount = TimeAccountType.NoWork;

                var dinnerState = new TimeState(title, description, "test", color, start, end, timeAccount);
                listOfStates.Add(dinnerState);

                start = end;
                end = start.AddMinutes(random.Next(7, 13) * 30);
                timeAccount = TimeAccountType.NormalWork;
                title = "In";
                description = "At the office";
                color = Colors.Green;

                if (end > DateTime.Today.AddHours(8 + 8))
                {
                    var overTimeEnd = end;
                    end = DateTime.Today.AddHours(8 + 8);
                    timeAccount = TimeAccountType.NormalWork;

                    var extraState = new TimeState(title, description, "test", color, start, end, timeAccount);
                    listOfStates.Add(extraState);

                    start = end;
                    end = overTimeEnd;
                    timeAccount = TimeAccountType.OvertimeWork;

                    var overtimeState = new TimeState("Busy", "Running an errand", "test", Colors.Blue, start, end, timeAccount);
                    listOfStates.Add(overtimeState);
                    continue;
                }
            }
            else if (i == 4)
            {
                start = end;
                end = DateTime.Today.AddHours(24);
                timeAccount = TimeAccountType.NoWork;
            }

            var state = new TimeState(title, description, "test", color, start, end, timeAccount);
            listOfStates.Add(state);
        }
        return listOfStates;
    }
}
