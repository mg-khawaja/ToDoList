using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ToDoList.Models;
using ToDoList.Pages;
using ToDoList.SQLiteDb;

namespace ToDoList.ViewModels
{
    public partial class GroceriesViewModel : BaseViewModel
    {
        #region properties
        GroceriesDatabase database;
        List<Grocery> allgroceries;
        int itemsFetched = 0;
        int itemsToFetch = 10;

        //bindable properties
        [ObservableProperty]
        List<Grocery> groceries;
        [ObservableProperty]
        bool isDataAvailable;
        #endregion

        public GroceriesViewModel(GroceriesDatabase database)
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
                await GetAllGroceries();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "Something went wrong! please try again later", "OK");
            }
            IsBusy = false;
        }
        [RelayCommand]
        public async Task DeleteGrocery(Grocery grocery)
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                await database.DeleteGrocery(grocery);
                await GetAllGroceries();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "Something went wrong! please try again later", "OK");
            }
            IsBusy = false;
        }
        [RelayCommand]
        public async Task GotoGrocery(Grocery grocery)
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                var navParam = new Dictionary<string, object>()
                {
                    {"grocery",grocery }
                };
                await Shell.Current.GoToAsync(nameof(GroceryPage), navParam);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "Something went wrong! please try again later", "OK");
            }
            IsBusy = false;
        }
        [RelayCommand]
        public async Task AddGrocery()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                await Shell.Current.GoToAsync(nameof(AddGroceryPage));
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "Something went wrong! please try again later", "OK");
            }
            IsBusy = false;
        }
        [RelayCommand]
        private async Task LoadMore(object obj)
        {
            if (!CanLoadMoreItems())
                return;
            IsBusy = true;
            await Task.Delay(2000);

            if (allgroceries.Count > itemsFetched + itemsToFetch)
            {
                itemsFetched += itemsToFetch;
                var list = new List<Grocery>(allgroceries.GetRange(0, itemsFetched));
                Groceries = list;
            }
            else
            {
                Groceries = new(allgroceries);
                itemsFetched = allgroceries.Count;
            }

            IsBusy = false;
        }
        #endregion

        #region methods
        private bool CanLoadMoreItems()
        {
            if (allgroceries == null || itemsFetched >= allgroceries.Count)
                return false;
            return true;
        }
        private async Task GetAllGroceries()
        {
            allgroceries = await database.GetAllGroceries();
            itemsFetched = 0;
            if (allgroceries != null && allgroceries.Count > 0)
            {
                if (allgroceries.Count > itemsFetched + itemsToFetch)
                {
                    itemsFetched += itemsToFetch;
                    var list = new List<Grocery>(allgroceries.GetRange(0, itemsFetched));
                    Groceries = list;
                }
                else
                {
                    Groceries = new(allgroceries);
                    itemsFetched = allgroceries.Count;
                }
            }
        }
        #endregion
    }
}
