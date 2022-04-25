using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using System.Diagnostics;

namespace Totalview_Time_Smartclient.Common.Services.Authentication;

internal interface IOidcService
{
    OidcClient CreateOidcClient();
}
internal class OidcService : IOidcService
{
    private readonly OidcOptions _openIdOptions;

    public OidcService(OidcOptions options)
    {
        _openIdOptions = options ?? throw new ArgumentNullException(nameof(options));
    }

    public OidcClient CreateOidcClient()
    {
        var oicdClientOptions = new OidcClientOptions
        {
            Authority = _openIdOptions.Authority,
            ClientId = OidcOptions.CliendId,
            Scope = OidcOptions.Scope,
            RedirectUri = OidcOptions.RedirectUri,
            ClientSecret = OidcOptions.ClientSecret,
            PostLogoutRedirectUri = OidcOptions.PostLogoutUri,
            Browser = new OidcBrowser()
        };

        var openIDClient = new OidcClient(oicdClientOptions);
        return openIDClient;
    }
}

public class OidcBrowser : IdentityModel.OidcClient.Browser.IBrowser
{
    public OidcBrowser() { }

    public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await WebAuthenticator.AuthenticateAsync(new Uri(options.StartUrl), new Uri(options.EndUrl));

            return new BrowserResult
            {
                Response = ToRawIdentityURL(options.EndUrl, result)
            };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);

            return new BrowserResult()
            {
                ResultType = BrowserResultType.UnknownError,
                Error = ex.ToString()
            };
        }
    }

    private string ToRawIdentityURL(string redirectURL, WebAuthenticatorResult result)
    {
        var parameters = result.Properties.Select(pair => $"{pair.Key}={pair.Value}");
        var values = string.Join("&", parameters);

        return $"{redirectURL}#{values}";
    }
}

internal record OidcOptions
{
    public readonly string Authority = "https://tvdev.formula.fo/TotalviewAuthentication";
    public const string CallbackUri = "fo.formula.totalview";
    public const string CliendId = "totalview-maui-totalview-time-client";
    public const string ClientName = "Totalview MAUI Totalview Time client";
    public const string Scope = "openid profile totalview-time-server offline_access";
    public const string RedirectUri = $"{CallbackUri}://oauth2redirect";
    public const string PostLogoutUri = $"{CallbackUri}://oauth2redirect";
    public const string ClientSecret = null;

    public OidcOptions(string authority)
    {
        Authority = authority ?? throw new ArgumentNullException(nameof(authority));
    }

    public OidcOptions()
    {

    }
}