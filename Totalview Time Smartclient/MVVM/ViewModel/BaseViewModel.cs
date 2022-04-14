using Totalview_Time_Smartclient.MVVM.Model.Services;

namespace Totalview_Time_Smartclient.MVVM.ViewModel;

public abstract class BaseViewModel : BindableObject
{
    private static IStorageService _storageService = StorageService.Instance;
    public BaseViewModel()
    {
    }
}
