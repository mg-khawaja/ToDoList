using ToDoList.Models;
using ToDoList.ViewModels;

namespace ToDoList.Pages;

public partial class GroceryPage : ContentPage
{
    GroceryViewModel viewModel;
	public GroceryPage(GroceryViewModel viewModel)
	{
		this.viewModel = viewModel;
		BindingContext = viewModel;
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
		viewModel.InitializeCommand.Execute(null);
    }
}