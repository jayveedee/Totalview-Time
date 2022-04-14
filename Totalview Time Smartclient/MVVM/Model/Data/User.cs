namespace Totalview_Time_Smartclient.MVVM.Model.Data;

internal record User
{
    public readonly long GUID;
    public readonly Name Name;
    public readonly List<TimeAccount> TimeAccounts;
    public readonly List<Registration> Registrations;

    public User(long gUID, Name name, List<TimeAccount> timeAccounts, List<Registration> registrations)
    {
        GUID = gUID;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        TimeAccounts = timeAccounts ?? throw new ArgumentNullException(nameof(timeAccounts));
        Registrations = registrations ?? throw new ArgumentNullException(nameof(registrations));
    }
}

internal record Name
{
    public readonly string First;
    public readonly string Last;
    public readonly string Full;

    public Name(string first, string last, string full)
    {
        First = first ?? throw new ArgumentNullException(nameof(first));
        Last = last ?? throw new ArgumentNullException(nameof(last));
        Full = full ?? throw new ArgumentNullException(nameof(full));
    }
}

internal record TimeAccount
{
    public readonly long Id;
    public readonly AccountName Name;
    public readonly List<Registration> Registrations;

    public TimeAccount(long id, AccountName name, List<Registration> registrations)
    {
        Id = id;
        Name = name;
        Registrations = registrations ?? throw new ArgumentNullException(nameof(registrations));
    }
}

internal enum AccountName
{
    Normal,
    Overtime,
    Vacation,
    Sickness,
    TimeOff
}

internal record Registration
{
    public readonly long Id;
    public readonly DateTime Date;
    public readonly List<State> States;
    public readonly bool Locked;
    public readonly Registration ModifiedRegistration;

    public Registration(long id, DateTime date, List<State> states, bool locked, Registration modifiedRegistration)
    {
        Id = id;
        Date = date;
        States = states ?? throw new ArgumentNullException(nameof(states));
        Locked = locked;
        ModifiedRegistration = modifiedRegistration;
    }
}

internal record State
{
    public readonly long Id;
    public readonly string Name;
    public readonly string Description;
    public readonly Color Color;
    public readonly DateTime StartTime;
    public readonly DateTime EndTime;

    public State(long id, string name, string description, Color color, DateTime startTime, DateTime endTime)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        Color = color ?? throw new ArgumentNullException(nameof(color));
        StartTime = startTime;
        EndTime = endTime;
    }
}