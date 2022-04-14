
/* Unmerged change from project 'Totalview Time Smartclient (net6.0)'
Before:
using Totalview_Time_Smartclient.MVVM.Model.Services.Authentication;
After:
using Totalview_Time_Smartclient;
using Totalview_Time_Smartclient.MVVM;
using Totalview_Time_Smartclient.MVVM.Model;
using Totalview_Time_Smartclient.MVVM.Model.Services.Authentication;
*/
using Totalview_Time_Smartclient.MVVM.Model.Services.Authentication;
using Totalview_Time_Smartclient.MVVM.Model.Util;

namespace Totalview_Time_Smartclient.MVVM.Model.Services;

internal interface IStorageService
{
    Task<StorageValues> UpdateStorageAsync();
    Task<T> GetStoredValueAsync<T>(string key);
    void SetStoredValueAsync<T>(string key, T value);
}
internal class StorageService : IStorageService
{
    private static IStorageService instance;

    private StorageService() { }

    public static IStorageService Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new StorageService();
            }
            return instance;
        }
    }

    public async Task<StorageValues> UpdateStorageAsync()
    {
        var authenticationCredentialsValue = await GetStoredValueAsync<AuthenticationCredentials>(StorageUtil.StorageKeys.AuthenticationCredentialsKey);
        var openIdOptionsValue = await GetStoredValueAsync<OpenIdOptions>(StorageUtil.StorageKeys.OpenIdOptionsKey);

        StorageUtil.StorageValues = new StorageValues(authenticationCredentialsValue, openIdOptionsValue);
        return StorageUtil.StorageValues;
    }

    public async Task<T> GetStoredValueAsync<T>(string key)
    {
        string storedString = await SecureStorage.GetAsync(key);
        var deSerializedObject = StorageUtil.DeserializeObjec<T>(storedString);
        return deSerializedObject;
    }

    public async void SetStoredValueAsync<T>(string key, T value)
    {
        var serializedObject = StorageUtil.SerializeObject(value);
        await SecureStorage.SetAsync(key, serializedObject);
        await UpdateStorageAsync();
    }
}
