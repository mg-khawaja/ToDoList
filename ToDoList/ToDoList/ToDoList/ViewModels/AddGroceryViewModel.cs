using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ToDoList.Models;
using ToDoList.SQLiteDb;

namespace ToDoList.ViewModels
{
    public partial class AddGroceryViewModel : BaseViewModel
    {
        #region properties
        GroceriesDatabase database;

        //bindable properties
        [ObservableProperty]
        string name;
        #endregion
        public AddGroceryViewModel(GroceriesDatabase database)
        {
            this.database = database;
        }

        #region commands
        [RelayCommand]
        public async Task Initialize()
        {
            try
            {
                IsBusy = true;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "Something went wrong! please try again later", "OK");
            }
            IsBusy = false;
        }
        [RelayCommand]
        public async Task SaveGrocery()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                if(!string.IsNullOrEmpty(Name))
                {
                    var grocery = new Grocery()
                    {
                        CreatedOn = DateTime.Now,
                        Name = Name,
                        Status = (Int32)Status.Pending
                    };
                    await database.InsertGrocery(grocery);

                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Please enter name", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "Something went wrong! please try again later", "OK");
            }
            IsBusy = false;
        }
        #endregion

        #region methods
        #endregion
    }
}
