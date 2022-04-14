namespace Totalview_Time_Smartclient.MVVM.Model.Services.Authentication;

internal record OpenIdOptions
{
    public readonly string Authority;
    public readonly string CliendId;
    public readonly string Scope;
    public readonly string RedirectUri;
    public readonly string PostLogoutUri;
    public readonly string ClientSecret;

    public OpenIdOptions(string authorityURL, string cliendID, string scope, string redirectUri, string postLogourUri, string clientSecret = null)
    {
        Authority = authorityURL ?? throw new ArgumentNullException(nameof(authorityURL));
        CliendId = cliendID ?? throw new ArgumentNullException(nameof(cliendID));
        Scope = scope ?? throw new ArgumentNullException(nameof(scope));
        RedirectUri = redirectUri ?? throw new ArgumentNullException(nameof(redirectUri));
        PostLogoutUri = postLogourUri ?? throw new ArgumentNullException(nameof(postLogourUri));
        ClientSecret = clientSecret;
    }
}
