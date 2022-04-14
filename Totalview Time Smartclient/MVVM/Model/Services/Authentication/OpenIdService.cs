using IdentityModel.OidcClient;

namespace Totalview_Time_Smartclient.MVVM.Model.Services.Authentication;

internal interface IOpenIdService
{
    OidcClient CreateOpenIdClient();
}
internal class OpenIdService : IOpenIdService
{
    private readonly OpenIdOptions _openIdOptions;

    public OpenIdService(OpenIdOptions openIdOptions)
    {
        _openIdOptions = openIdOptions ?? throw new ArgumentNullException(nameof(openIdOptions));
    }

    public OidcClient CreateOpenIdClient()
    {
        var openIDClientOptions = new OidcClientOptions
        {
            Authority = _openIdOptions.Authority,
            ClientId = _openIdOptions.CliendId,
            Scope = _openIdOptions.Scope,
            RedirectUri = _openIdOptions.RedirectUri,
            ClientSecret = _openIdOptions.ClientSecret,
            PostLogoutRedirectUri = _openIdOptions.PostLogoutUri,
            Browser = new AuthenticationBrowser()
        };

        var openIDClient = new OidcClient(openIDClientOptions);
        return openIDClient;
    }
}