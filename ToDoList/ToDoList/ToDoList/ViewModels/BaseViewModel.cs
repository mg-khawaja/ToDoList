using CommunityToolkit.Mvvm.ComponentModel;

namespace ToDoList.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        //IsBusy binded with the ActivityIndicator to show busy state on each page
        [ObservableProperty]
        bool isBusy;

        //Title binded with the each page title
        [ObservableProperty]
        string title;
    }
}
