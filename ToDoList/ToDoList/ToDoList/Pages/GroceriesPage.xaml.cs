using ToDoList.Models;
using ToDoList.ViewModels;

namespace ToDoList.Pages;

public partial class GroceriesPage : ContentPage
{
	GroceriesViewModel viewModel;
	public GroceriesPage(GroceriesViewModel viewModel)
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