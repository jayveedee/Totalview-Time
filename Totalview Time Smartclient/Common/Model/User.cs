namespace Totalview_Time_Smartclient.Common.Model;

internal record User
{
    public readonly int ID;
    public readonly string Name;

    public User(int id, string name)
    {
        ID = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }
}
