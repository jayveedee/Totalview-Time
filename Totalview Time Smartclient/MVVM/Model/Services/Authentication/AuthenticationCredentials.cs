namespace Totalview_Time_Smartclient.MVVM.Model.Services.Authentication;

internal record AuthenticationCredentials
{
    public readonly bool IsValid;
    public readonly string AccessToken;
    public readonly string IdentityToken;
    public readonly string RefreshToken;
    public readonly DateTimeOffset AccessTokenExpiration;

    public AuthenticationCredentials(bool isValid, string accessToken, string identityToken, string refreshToken, DateTimeOffset accessTokenExpiration)
    {
        IsValid = isValid;
        AccessToken = accessToken;
        IdentityToken = identityToken;
        RefreshToken = refreshToken;
        AccessTokenExpiration = accessTokenExpiration;
        if (isValid)
        {
            AccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
            IdentityToken = identityToken ?? throw new ArgumentNullException(nameof(identityToken));
            RefreshToken = refreshToken ?? throw new ArgumentNullException(nameof(refreshToken));
        }
    }
}
