using TimeServerAPI;
using Totalview_Time_MAUI.Common.Model.TimeManagement;

namespace Totalview_Time_MAUI.Common.Services.TimeServerAPIService;

internal interface ITimeServerAPIService
{
    void Initialize(WCFDatabaseClient apiClient);
    void GetAgreements(DateTime date, int userId);
    Task<List<TimeRegistration>> GetTimeRegistrations();
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
        throw new NotImplementedException();
    }

    public Task<List<TimeRegistration>> GetTimeRegistrations()
    {
        throw new NotImplementedException();
    }
}
