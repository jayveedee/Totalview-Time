namespace Totalview_Time_MAUI;

internal class ToolbarItemCustom : ToolbarItem
{
    public static readonly BindableProperty IsVisibleProperty =
        BindableProperty.Create(nameof(IsVisible), typeof(bool), typeof(ToolbarItemCustom), true, BindingMode.OneWay, propertyChanged: OnIsVisibleChanged);

    public bool IsVisible
    {
        get => (bool)GetValue(IsVisibleProperty);
        set => SetValue(IsVisibleProperty, value);
    }

    protected override void OnParentChanged()
    {
        base.OnParentChanged();

        RefreshVisibility();
    }

    private static void OnIsVisibleChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        var item = bindable as ToolbarItemCustom;

        item.RefreshVisibility();
    }

    private void RefreshVisibility()
    {
        if (Parent == null)
        {
            return;
        }

        bool value = IsVisible;

        var toolbarItems = ((ContentPage)Parent).ToolbarItems;

        if (value && !toolbarItems.Contains(this))
        {
            Application.Current.Dispatcher.Dispatch(() => { toolbarItems.Add(this); });
        }
        else if (!value && toolbarItems.Contains(this))
        {
            Application.Current.Dispatcher.Dispatch(() => { toolbarItems.Remove(this); });
        }
    }
}