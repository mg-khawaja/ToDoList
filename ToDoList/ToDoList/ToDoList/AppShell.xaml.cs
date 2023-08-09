using ToDoList.Pages;

namespace ToDoList;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        Routing.RegisterRoute(nameof(GroceryPage), typeof(GroceryPage));
        Routing.RegisterRoute(nameof(AddItemPage), typeof(AddItemPage));
        Routing.RegisterRoute(nameof(AddGroceryPage), typeof(AddGroceryPage));
    }
}
