using IdentityModel.OidcClient;
using System.Diagnostics;
using Totalview_Time_Smartclient.MVVM.Model.Util;

namespace Totalview_Time_Smartclient.MVVM.Model.Services.Authentication;

internal interface IAuthenticationService
{
    Task<AuthenticationCredentials> SignIn(CancellationToken cancellationToken = default);
    Task<AuthenticationCredentials> SignOut(AuthenticationCredentials credentials, CancellationToken cancellationToken = default);
    Task<AuthenticationCredentials> RefreshSession(AuthenticationCredentials credentials, CancellationToken cancellationToken = default);
}

internal class AuthenticationService : IAuthenticationService
{
    private readonly IOpenIdService _openIdService;

    public AuthenticationService(OpenIdOptions openIdOptions)
    {
        if (openIdOptions == null)
        {
            throw new ArgumentNullException(nameof(openIdOptions));
        }
        _openIdService = new OpenIdService(openIdOptions);
    }

    public async Task<AuthenticationCredentials> SignIn(CancellationToken cancellationToken = default)
    {
        AuthenticationCredentials credentials;
        try
        {
            var openIdClient = _openIdService.CreateOpenIdClient();
            var loginResult = await openIdClient.LoginAsync(new LoginRequest(), cancellationToken);

            credentials = new AuthenticationCredentials(
                !loginResult.IsError,
                loginResult.AccessToken, 
                loginResult.IdentityToken, 
                loginResult.RefreshToken, 
                loginResult.AccessTokenExpiration);
        }
        //TODO: Handle different types of exceptions (e.g. Timeout, SSL, UnknownHost etc.)
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            credentials = new AuthenticationCredentials(false, null, null, null, new DateTimeOffset());
        }
        StorageService.Instance.SetStoredValueAsync<AuthenticationCredentials>(StorageUtil.StorageKeys.AuthenticationCredentialsKey, credentials);
        return credentials;
    }

    public async Task<AuthenticationCredentials> SignOut(AuthenticationCredentials credentials, CancellationToken cancellationToken = default)
    {
        credentials = credentials ?? throw new ArgumentNullException(nameof(credentials));

        var openIDClient = _openIdService.CreateOpenIdClient();
        var logoutResult = await openIDClient.LogoutAsync(new LogoutRequest { IdTokenHint = credentials?.IdentityToken }, cancellationToken);

        if (logoutResult != null && !logoutResult.IsError)
        {
            credentials = new AuthenticationCredentials(false, null, null, null, new DateTimeOffset());
            StorageService.Instance.SetStoredValueAsync<AuthenticationCredentials>(StorageUtil.StorageKeys.AuthenticationCredentialsKey, credentials);
        }

        Debug.WriteLine(logoutResult.Error);
        return credentials;
    }

    public async Task<AuthenticationCredentials> RefreshSession(AuthenticationCredentials credentials, CancellationToken cancellationToken = default)
    {
        credentials = credentials ?? throw new ArgumentNullException(nameof(credentials));

        try
        {
            var openIDClient = _openIdService.CreateOpenIdClient();
            var refreshResult = await openIDClient.RefreshTokenAsync(credentials?.RefreshToken, cancellationToken: cancellationToken);

            credentials = new AuthenticationCredentials(
                !refreshResult.IsError,
                refreshResult.AccessToken,
                refreshResult.IdentityToken,
                refreshResult.RefreshToken,
                refreshResult.AccessTokenExpiration);
        }
        //TODO: Handle different types of exceptions (e.g. Timeout, SSL, UnknownHost etc.)
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            credentials = new AuthenticationCredentials(false, null, null, null, new DateTimeOffset()); ;
        }

        StorageService.Instance.SetStoredValueAsync<AuthenticationCredentials>(StorageUtil.StorageKeys.AuthenticationCredentialsKey, credentials);
        return credentials;
    }

}
