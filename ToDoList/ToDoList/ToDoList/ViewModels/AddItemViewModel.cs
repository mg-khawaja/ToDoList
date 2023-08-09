using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ToDoList.Models;
using ToDoList.SQLiteDb;

namespace ToDoList.ViewModels
{
    public partial class AddItemViewModel : BaseViewModel, IQueryAttributable
    {
        #region properties
        GroceriesDatabase database;
        Grocery grocery;

        //bindable properties
        [ObservableProperty]
        string name;
        [ObservableProperty]
        string price;
        #endregion
        public AddItemViewModel(GroceriesDatabase database)
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
        public async Task SaveItem()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                if(!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Price))
                {
                    var item = new ToDoItem()
                    {
                        CreatedOn = DateTime.Now,
                        Name = Name,
                        Price = Convert.ToDouble(Price),
                        GroceryId = grocery.Id,
                        Status = (Int32)Status.Pending
                    };
                    await database.InsertItem(item);
                    
                    grocery.Status = (Int32)Status.Pending;
                    await database.UpdateGrocery(grocery);

                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "All fields are required", "OK");
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
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            grocery = query["grocery"] as Grocery;
        }
        #endregion
    }
}
