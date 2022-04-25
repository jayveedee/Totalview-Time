using TimeServerAPI;
using Totalview_Time_Smartclient.Common.Util;

namespace Totalview_Time_Smartclient.Common.Services.TimeServerAPIService;

internal interface ITimeServerAPIService
{
    void Initialize(WCFDatabaseClient apiClient);
    void GetAgreements(DateTime date, int userId);
}

internal class TimeServerAPIService : ITimeServerAPIService
{
    private WCFDatabaseClient _apiClient;
    private static ITimeServerAPIService instance;

    public static ITimeServerAPIService Instance
    {
        get {
            if (instance == null)
            {
                instance = new TimeServerAPIService();
            }
            return instance;
        }
    }
    public void Initialize(WCFDatabaseClient apiClient)
    {
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    public async void GetAgreements(DateTime date, int userId)
    {
        var agreement = await _apiClient.GetAgreementAsync(date, userId);
    }
}
