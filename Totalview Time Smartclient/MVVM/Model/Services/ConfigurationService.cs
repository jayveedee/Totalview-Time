namespace Totalview_Time_Smartclient.MVVM.Model.Services;

internal interface IConfigurationService
{
    IFontCollection ConfigureAppFonts(IFontCollection fonts);
}
internal class ConfigurationService : IConfigurationService
{
    private readonly Dictionary<string, string> _appFonts = CreateFontList();

    public ConfigurationService() { }

    public IFontCollection ConfigureAppFonts(IFontCollection fonts)
    {
        IFontCollection configuredFonts = fonts;
        foreach (KeyValuePair<string, string> pair in _appFonts)
        {
            configuredFonts.AddFont(pair.Key, pair.Value);
        }
        return configuredFonts;
    }

    private static Dictionary<string, string> CreateFontList()
    {
        Dictionary<string, string> fontList = new Dictionary<string, string>();
        fontList.Add("Roboto-Black.ttf", "RobotoBlack");
        fontList.Add("Roboto-BlackItalic.ttf", "RobotoBlackItalic");
        fontList.Add("Roboto-Bold.ttf", "RobotoBold");
        fontList.Add("Roboto-BoldItalic.ttf", "RobotoBoldItalic");
        fontList.Add("Roboto-Italic.ttf", "RobotoItalic");
        fontList.Add("Roboto-Light.ttf", "RobotoLight");
        fontList.Add("Roboto-LightItalic.ttf", "RobotoLightItalic");
        fontList.Add("Roboto-Medium.ttf", "RobotoMedium");
        fontList.Add("Roboto-MediumItalic.ttf", "RobotoMediumItalic");
        fontList.Add("Roboto-Regular.ttf", "RobotoRegular");
        fontList.Add("Roboto-Thin.ttf", "RobotoThin");
        fontList.Add("Roboto-ThinItalic.ttf", "RobotoThinItalic");
        return fontList;
    }
}
