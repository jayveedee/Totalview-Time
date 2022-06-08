using Totalview_Time_MAUI.Common.Model;
using Totalview_Time_MAUI.Common.Model.TimeManagement;
using Totalview_Time_MAUI.Common.Services.Authentication;
using Totalview_Time_MAUI.Common.Util;

namespace Totalview_Time_MAUI.Common.Services;

internal interface IStorageService
{
    void SaveStorage(Storage storage);
    Storage LoadStorage();
    void SaveStorageAsync(Storage storage = default);
    Task<Storage> LoadStorageAsync();
    void ClearStorage();
    bool StorageIsUpToDate();
}
internal class StorageService : IStorageService
{
    private static bool _localStorageIsUpToDate;
    private static Storage _storage;
    private static IStorageService _instance;

    private StorageService() 
    {
        _storage = new Storage();
    }

    public static IStorageService Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new StorageService();
            }
            return _instance;
        }
    }

    public void SaveStorage(Storage storage)
    {
        _localStorageIsUpToDate = false;
        _storage = storage;
        SaveStorageAsync();
    }

    public Storage LoadStorage()
    {
        return _storage;
    }

    public async void SaveStorageAsync(Storage storage = default)
    {
        _localStorageIsUpToDate = true;

        var serializedObject = StorageUtil.SerializeObject(storage != default ? storage : _storage);
        await SecureStorage.SetAsync(StorageUtil.StorageKey, serializedObject);
    }

    public async Task<Storage> LoadStorageAsync()
    {
        var serializedObject = await SecureStorage.GetAsync(StorageUtil.StorageKey);

        if (string.IsNullOrEmpty(serializedObject)) return default;

        _storage = StorageUtil.DeserializeObjec<Storage>(serializedObject);
        return _storage;
    }

    public void ClearStorage()
    {
        _localStorageIsUpToDate = true;
        SecureStorage.RemoveAll();
    }

    public bool StorageIsUpToDate()
    {
        return _localStorageIsUpToDate;
    }
}

internal record Storage
{
    public AuthCredentials AuthCredentials;
    public OidcOptions OidcOptions;
    public User User;
    public List<TimeRegistration> TimeRegistrations;

    public Storage() { }
}