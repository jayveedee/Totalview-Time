namespace Totalview_Time_Smartclient.MVVM.Model.Services;

public interface IAppConfigurationService
{
    void AddFonts(IFontCollection fonts);
}
public class AppConfigurationService : IAppConfigurationService
{
    public void AddFonts(IFontCollection fonts)
    {
        fonts.AddFont("Roboto-Black.ttf", "RobotoBlack");
        fonts.AddFont("Roboto-BlackItalic.ttf", "RobotoBlackItalic");
        fonts.AddFont("Roboto-Bold.ttf", "RobotoBold");
        fonts.AddFont("Roboto-BoldItalic.ttf", "RobotoBoldItalic");
        fonts.AddFont("Roboto-Italic.ttf", "RobotoItalic");
        fonts.AddFont("Roboto-Light.ttf", "RobotoLight");
        fonts.AddFont("Roboto-LightItalic.ttf", "RobotoLightItalic");
        fonts.AddFont("Roboto-Medium.ttf", "RobotoMedium");
        fonts.AddFont("Roboto-MediumItalic.ttf", "RobotoMediumItalic");
        fonts.AddFont("Roboto-Regular.ttf", "RobotoRegular");
        fonts.AddFont("Roboto-Thin.ttf", "RobotoThin");
        fonts.AddFont("Roboto-ThinItalic.ttf", "RobotoThinItalic");
    }
}
