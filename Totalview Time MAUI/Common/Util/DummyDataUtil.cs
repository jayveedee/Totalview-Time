using Totalview_Time_MAUI.Common.Model.TimeRegistration;

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

    public static List<Registration> CreateTimeRegistrations(int amount = 15)
    {
        var random = new Random();
        var listOfRegistrations = new List<Registration>();

        var enumValues = Enum.GetValues(typeof(EditState));
        for (int i = 0; i < amount; i++)
        {
            var day = DateTime.Today.AddDays(-i + 1);
            var registration = new Registration(day, CreateTimeStates(day), (EditState)enumValues.GetValue(random.Next(enumValues.Length)));
            listOfRegistrations.Add(registration);
        }
        return listOfRegistrations;
    }

    public static Registration CreateTimeRegistration()
    {
        var yesterday = DateTime.Today.AddDays(-1);
        var registration = new Registration(yesterday, CreateTimeStates(yesterday), EditState.Open);
        return registration;
    }

    public static List<State> CreateStateList()
    {
        var states = new List<State>();
        for (int i = 0; i < _listOfTitles.Count; i++)
        {
            var state = new State(_listOfTitles[i], _listOfDescriptions[i], _listOfColors[i], DateTime.Now, DateTime.Now, TimeAccount.NoWork);
            states.Add(state);
        }
        states.Sort((x, y) => x.Title.CompareTo(y.Title));
        return states;
    }

    public static List<State> CreateTimeStates(DateTime day, int amount = 5)
    {
        var random = new Random();
        var listOfStates = new List<State>();

        var start = day;
        var end = DateTime.Today.AddHours(8);
        for (int i = 0; i < amount; i++)
        {
            var title = _listOfTitles[i];
            var description = _listOfDescriptions[i];
            var color = _listOfColors[i];
            var timeAccount = TimeAccount.NoWork;
            
            if (i == 1)
            {
                start = end;
                end = start.AddMinutes(random.Next(1, 4) * 30);
                timeAccount = TimeAccount.NormalWork;
            }
            else if (i == 2)
            {
                start = end;
                end = DateTime.Today.AddHours(12);
                timeAccount = TimeAccount.NormalWork;
            }
            else if (i == 3)
            {
                start = end;
                var breakTime = random.Next(1, 2) * 30;
                end = start.AddMinutes(breakTime);
                timeAccount = TimeAccount.NoWork;

                var dinnerState = new State(title, description, color, start, end, timeAccount);
                listOfStates.Add(dinnerState);

                start = end;
                end = start.AddMinutes(random.Next(7, 13) * 30);
                timeAccount = TimeAccount.NormalWork;
                title = "In";
                description = "At the office";
                color = Colors.Green;

                if (end > DateTime.Today.AddHours(8 + 8))
                {
                    var overTimeEnd = end;
                    end = DateTime.Today.AddHours(8 + 8);
                    timeAccount = TimeAccount.NormalWork;

                    var extraState = new State(title, description, color, start, end, timeAccount);
                    listOfStates.Add(extraState);

                    start = end;
                    end = overTimeEnd;
                    timeAccount = TimeAccount.OvertimeWork;

                    var overtimeState = new State("Busy", "Running an errand", Colors.Blue, start, end, timeAccount);
                    listOfStates.Add(overtimeState);
                    continue;
                }
            }
            else if (i == 4)
            {
                start = end;
                end = DateTime.Today.AddHours(24);
                timeAccount = TimeAccount.NoWork;
            }

            var state = new State(title, description, color, start, end, timeAccount);
            listOfStates.Add(state);
        }
        return listOfStates;
    }
}
