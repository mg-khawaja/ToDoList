using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ToDoList.Models;
using ToDoList.Pages;
using ToDoList.SQLiteDb;

namespace ToDoList.ViewModels
{
    public partial class HomeViewModel : BaseViewModel
    {
        #region properties
        GroceriesDatabase database;
        Grocery groceryAllItem;
        int itemsFetched = 0;
        int itemsToFetch = 10;

        //bindable properties
        [ObservableProperty]
        Grocery grocery;
        [ObservableProperty]
        bool isDataAvailable;
        #endregion
        public HomeViewModel(GroceriesDatabase database)
        {
            this.database = database;
            Title = "Home";
        }

        #region commands
        //IRelayCommand<object> LoadMoreItemsCommand = new RelayCommand<object>(LoadMoreItems, CanLoadMoreItems);
        [RelayCommand]
        public async Task Initialize()
        {
            try
            {
                IsBusy = true;
                await GetLatestPendingGrocery();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "Something went wrong! please try again later", "OK");
            }
            IsBusy = false;
        }
        [RelayCommand]
        public async Task ItemDone(ToDoItem item)
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;

                item.Status = (Int32)Status.Completed;
                await database.UpdateItem(item);

                if (Grocery.Items.Count == 1)
                {
                    Grocery.Status = (Int32)Status.Completed;
                }
                await database.UpdateGrocery(Grocery);

                await GetLatestPendingGrocery();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "Something went wrong! please try again later", "OK");
            }
            IsBusy = false;
        }
        [RelayCommand]
        public async Task AddItem()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                var navParam = new Dictionary<string, object>()
                {
                    {"grocery", Grocery }
                };
                await Shell.Current.GoToAsync(nameof(AddItemPage), navParam);
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

            if (groceryAllItem != null && groceryAllItem.Items != null &&
                groceryAllItem.Items.Count > itemsFetched + itemsToFetch)
            {
                var newGrocery = new Grocery()
                {
                    Id = groceryAllItem.Id,
                    CreatedOn = groceryAllItem.CreatedOn,
                    Name = groceryAllItem.Name,
                    Status = groceryAllItem.Status,
                };
                itemsFetched += itemsToFetch;
                newGrocery.Items = groceryAllItem.Items.GetRange(0, itemsFetched);
                Grocery = newGrocery;
            }
            else
            {
                Grocery = groceryAllItem;
                itemsFetched = groceryAllItem.Items.Count;
            }

            IsBusy = false;
        }
        #endregion

        #region methods
        private async Task GetLatestPendingGrocery()
        {
            groceryAllItem = await database.GetLatestPendingGrocery();
            itemsFetched = 0;
            if (groceryAllItem == null)
                Title = "Home";
            else
            {
                Title = groceryAllItem.Name;
                if (groceryAllItem.Items != null && groceryAllItem.Items.Count > itemsFetched + itemsToFetch)
                {
                    var newGrocery = new Grocery()
                    {
                        Id = groceryAllItem.Id,
                        CreatedOn = groceryAllItem.CreatedOn,
                        Name = groceryAllItem.Name,
                        Status = groceryAllItem.Status,
                    };
                    itemsFetched += itemsToFetch;
                    newGrocery.Items = groceryAllItem.Items.GetRange(0, itemsFetched);
                    Grocery = newGrocery;
                }
                else
                {
                    Grocery = groceryAllItem;
                    itemsFetched = groceryAllItem.Items.Count;
                }
            }
        }

        private bool CanLoadMoreItems()
        {
            if (groceryAllItem == null || groceryAllItem.Items == null ||
                itemsFetched >= groceryAllItem.Items.Count)
                return false;
            return true;
        }
        #endregion
    }
}
