using IdentityModel.OidcClient;
using System.Diagnostics;

namespace Totalview_Time_MAUI.Common.Services.Authentication;

internal interface IAuthenticationService
{
    Task<AuthCredentials> SignIn(CancellationToken cancellationToken = default);
    Task<AuthCredentials> SignOut(AuthCredentials credentials, CancellationToken cancellationToken = default);
    Task<AuthCredentials> RefreshSession(AuthCredentials credentials, CancellationToken cancellationToken = default);
}

internal class AuthenticationService : IAuthenticationService
{
    private readonly OidcClient _oidcClient;
    private readonly IRefreshSessionService _refreshSessionService;

    public AuthenticationService(OidcOptions oicdOptions)
    {
        if (oicdOptions == null)
        {
            throw new ArgumentNullException(nameof(oicdOptions));
        }
        _oidcClient = new OidcService(oicdOptions).CreateOidcClient();
        _refreshSessionService = new RefreshSessionService(this);
    }

    public async Task<AuthCredentials> SignIn(CancellationToken cancellationToken = default)
    {
        AuthCredentials credentials;
        try
        {
            var loginResult = await _oidcClient.LoginAsync(new LoginRequest(), cancellationToken);

            credentials = new AuthCredentials(
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
            credentials = new AuthCredentials(false, null, null, null, new DateTimeOffset());
        }
        SaveCredentialsToStorage(credentials);
        if (credentials.IsValid)
        {
            _refreshSessionService.StartRefreshing(credentials);
        }
        return credentials;
    }

    public async Task<AuthCredentials> SignOut(AuthCredentials credentials, CancellationToken cancellationToken = default)
    {
        credentials = credentials ?? throw new ArgumentNullException(nameof(credentials));

        var logoutResult = await _oidcClient.LogoutAsync(new LogoutRequest { IdTokenHint = credentials?.IdentityToken }, cancellationToken);

        if (logoutResult != null && !logoutResult.IsError)
        {
            credentials = new AuthCredentials(false, null, null, null, new DateTimeOffset());
            SaveCredentialsToStorage(credentials);
            _refreshSessionService.StopRefreshing();
        }

        Debug.WriteLine(logoutResult.Error);
        return credentials;
    }

    public async Task<AuthCredentials> RefreshSession(AuthCredentials credentials, CancellationToken cancellationToken = default)
    {
        credentials = credentials ?? throw new ArgumentNullException(nameof(credentials));

        try
        {
            var refreshResult = await _oidcClient.RefreshTokenAsync(credentials?.RefreshToken, cancellationToken: cancellationToken);

            credentials = new AuthCredentials(
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
            credentials = new AuthCredentials(false, null, null, null, new DateTimeOffset()); ;
        }

        SaveCredentialsToStorage(credentials);
        _refreshSessionService.UpdateRefresher(credentials);
        return credentials;
    }

    private void SaveCredentialsToStorage(AuthCredentials credentials)
    {
        var storage = StorageService.Instance.LoadStorage();
        storage.AuthCredentials = credentials;
        StorageService.Instance.SaveStorage(storage);
    }
}

internal record AuthCredentials
{
    public readonly bool IsValid;
    public readonly string AccessToken;
    public readonly string IdentityToken;
    public readonly string RefreshToken;
    public readonly DateTimeOffset AccessTokenExpiration;

    public AuthCredentials(bool isValid, string accessToken, string identityToken, string refreshToken, DateTimeOffset accessTokenExpiration)
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
