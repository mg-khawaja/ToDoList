using SQLite;
using ToDoList.Helpers;
using ToDoList.Models;

namespace ToDoList.SQLiteDb
{
    public class GroceriesDatabase
    {
        SQLiteAsyncConnection Database;

        public GroceriesDatabase()
        {
        }
        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            await Database.CreateTableAsync<Grocery>();
            await Database.CreateTableAsync<ToDoItem>();

            //await InsertDemoData();
        }
        public async Task<Grocery> GetGrocery(int groceryId)
        {
            await Init();
            var grocery = await Database.Table<Grocery>().Where(t => t.Id == groceryId).FirstOrDefaultAsync();
            grocery.Items = await this.GetPendingGroceryItems(groceryId);
            return grocery;
        }
        public async Task<List<ToDoItem>> GetGroceryItems(int groceryId)
        {
            await Init();
            return await Database.Table<ToDoItem>().Where(t => t.GroceryId == groceryId).ToListAsync();
        }
        public async Task<List<ToDoItem>> GetPendingGroceryItems(int groceryId)
        {
            await Init();
            var list = await Database.Table<ToDoItem>().Where(t => t.GroceryId == groceryId && t.Status == (Int32)Status.Pending).ToListAsync();
            return list.OrderByDescending(o => o.CreatedOn).ToList();
        }
        public async Task<Grocery> GetLatestPendingGrocery()
        {
            await Init();
            var list = await Database.Table<Grocery>().Where(g => g.Status == (Int32)Status.Pending).ToListAsync();
            if (list == null || list.Count == 0)
                return null;
            var grocery = list.LastOrDefault();
            var items = await this.GetPendingGroceryItems(grocery.Id);
            grocery.Items = items.ToList();
            return grocery;
        }
        public async Task<List<Grocery>> GetAllGroceries()
        {
            await Init();
            var groceries = await Database.Table<Grocery>().ToListAsync();
            groceries = groceries.OrderByDescending(o=> o.CreatedOn).ToList();
            foreach (var grocery in groceries)
            {
                var items = await this.GetGroceryItems(grocery.Id);
                grocery.Items = items;
            }
            return groceries;
        }
        public async Task InsertGrocery(Grocery grocery)
        {
            await Init();
            await Database.InsertAsync(grocery);
            var list = await Database.Table<Grocery>().ToListAsync();
            var insertedGroc = list.LastOrDefault();
            if (grocery.Items != null && grocery.Items.Count > 0)
            {
                foreach (var item in grocery.Items)
                {
                    item.GroceryId = insertedGroc.Id;
                }
                await Database.InsertAllAsync(grocery.Items);
            }
        }
        public async Task UpdateGrocery(Grocery grocery)
        {
            await Init();
            await Database.UpdateAsync(grocery);
        }
        public async Task DeleteGrocery(Grocery grocery)
        {
            await Init();
            await Database.RunInTransactionAsync(async (val) =>
            {
                foreach (var item in grocery.Items)
                {
                    await Database.DeleteAsync(item);
                }
                await Database.DeleteAsync(grocery);
            });
        }
        public async Task UpdateItem(ToDoItem item)
        {
            await Init();
            await Database.UpdateAsync(item);
        }
        public async Task DeleteItem(ToDoItem item)
        {
            await Init();
            await Database.DeleteAsync(item);
        }
        public async Task InsertItem(ToDoItem item)
        {
            await Init();
            await Database.InsertAsync(item);
        }

        public async Task InsertDemoData()
        {
            await this.InsertGrocery(new Grocery()
            {
                CreatedOn = DateTime.Now,
                Name = "Grocery 1",
                Status = (Int32)Status.Pending,
                Items = new List<ToDoItem>()
                {
                    new ToDoItem { CreatedOn = DateTime.Now, Name = "Item 1", Price = 12.5F, Status = (Int32)Status.Pending },
                    new ToDoItem { CreatedOn = DateTime.Now, Name = "Item 2", Price = 12.5F, Status = (Int32)Status.Pending },
                    new ToDoItem { CreatedOn = DateTime.Now, Name = "Item 3", Price = 12.5F, Status = (Int32)Status.Pending },
                }
            });
            await this.InsertGrocery(new Grocery()
            {
                CreatedOn = DateTime.Now,
                Name = "Grocery 2",
                Status = (Int32)Status.Pending,
                Items = new List<ToDoItem>()
                {
                    new ToDoItem { CreatedOn = DateTime.Now, Name = "Item 1", Price = 12.5F, Status = (Int32)Status.Pending },
                    new ToDoItem { CreatedOn = DateTime.Now, Name = "Item 2", Price = 12.5F, Status = (Int32)Status.Pending },
                    new ToDoItem { CreatedOn = DateTime.Now, Name = "Item 3", Price = 12.5F, Status = (Int32)Status.Pending },
                }
            });
            await this.InsertGrocery(new Grocery()
            {
                CreatedOn = DateTime.Now,
                Name = "Grocery 3",
                Status = (Int32)Status.Pending,
                Items = new List<ToDoItem>()
                {
                    new ToDoItem { CreatedOn = DateTime.Now, Name = "Item 1", Price = 12.5F, Status = (Int32)Status.Pending },
                    new ToDoItem { CreatedOn = DateTime.Now, Name = "Item 2", Price = 12.5F, Status = (Int32)Status.Pending },
                    new ToDoItem { CreatedOn = DateTime.Now, Name = "Item 3", Price = 12.5F, Status = (Int32)Status.Pending },
                }
            });
        }
    }
}
