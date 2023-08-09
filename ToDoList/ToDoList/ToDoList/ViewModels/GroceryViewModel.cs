using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ToDoList.Models;
using ToDoList.Pages;
using ToDoList.SQLiteDb;

namespace ToDoList.ViewModels
{
    public partial class GroceryViewModel : BaseViewModel, IQueryAttributable
    {
        #region properties
        GroceriesDatabase database;
        Grocery groceryAllItem;
        int itemsFetched = 0;
        int itemsToFetch = 10;
        bool pageInitialized = false;

        //bindable properties
        [ObservableProperty]
        Grocery grocery;
        [ObservableProperty]
        bool isDataAvailable;
        #endregion
        public GroceryViewModel(GroceriesDatabase database)
        {
            this.database = database;
            Title = "Home";
        }

        #region commands
        [RelayCommand]
        public async Task Initialize()
        {
            try
            {
                IsBusy = true;
                if (groceryAllItem != null)
                    await GetGrocery(groceryAllItem.Id);
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

                await GetGrocery(Grocery.Id);
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
        private async Task GetGrocery(int groceryId)
        {
            groceryAllItem = await database.GetGrocery(groceryId);
            if (groceryAllItem == null)
                Title = "Detail";
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

            pageInitialized = true;
        }
        private bool CanLoadMoreItems()
        {
            if (groceryAllItem == null || groceryAllItem.Items == null || !pageInitialized ||
                itemsFetched >= groceryAllItem.Items.Count)
                return false;
            return true;
        }
        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            groceryAllItem = query["grocery"] as Grocery;
        }
        #endregion
    }
}
