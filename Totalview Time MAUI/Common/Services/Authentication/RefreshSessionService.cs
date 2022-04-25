namespace Totalview_Time_MAUI.Common.Services.Authentication;

internal interface IRefreshSessionService
{
    void StartRefreshing(AuthCredentials authCredentials, int intervalSeconds = 60);
    void UpdateRefresher(AuthCredentials authCredentials, int intervalSeconds = 60);
    void StopRefreshing();
}

internal class RefreshSessionService : IRefreshSessionService
{
    private AuthenticationService _authenticationService;
    private AuthCredentials _authCredentials;
    private static Timer _timer;
    private static bool _isDoingWorkFlag;
    private int _interval;

    public RefreshSessionService(AuthenticationService authenticationService)
    {
        _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
    }

    public void StartRefreshing(AuthCredentials authCredentials, int intervalSeconds = 60)
    {
        _interval = intervalSeconds;
        if (_timer != null)
        {
            throw new InvalidOperationException("Timer already started");
        }
        _authCredentials = authCredentials ?? throw new ArgumentNullException(nameof(authCredentials));
        _timer = new Timer(RefreshSessionChecker, null, TimeSpan.Zero, TimeSpan.FromSeconds(_interval));
    }

    public void UpdateRefresher(AuthCredentials authCredentials, int intervalSeconds = 60)
    {
        _interval = intervalSeconds;
        if (_timer == null)
        {
            throw new InvalidOperationException("Timer not started");
        }
        _authCredentials = authCredentials ?? throw new ArgumentNullException(nameof(authCredentials));
        if (intervalSeconds != 60)
        {
            _timer.Change(TimeSpan.Zero, TimeSpan.FromSeconds(_interval));
        }
    }

    public void StopRefreshing()
    {
        if (_timer == null)
        {
            throw new InvalidOperationException("Timer not started");
        }
        _timer.Dispose();
        _timer = null;
    }

    private async void RefreshSessionChecker(object state)
    {
        DateTime dateNow = DateTime.Now;
        if (dateNow > _authCredentials.AccessTokenExpiration)
        {
            if (!_isDoingWorkFlag)
            {
                _isDoingWorkFlag = true;
                AuthCredentials credentials = await _authenticationService.RefreshSession(_authCredentials);
                if (!credentials.IsValid)
                {
                    await Shell.Current.GoToAsync($"//{nameof(LoginServerDetails)}");
                    StopRefreshing();
                    _isDoingWorkFlag = false;
                    return;
                }
                _isDoingWorkFlag = false;
            }
        }
    }
}
