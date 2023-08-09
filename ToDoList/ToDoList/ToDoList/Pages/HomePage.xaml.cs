using ToDoList.Models;
using ToDoList.ViewModels;

namespace ToDoList.Pages;

public partial class HomePage : ContentPage
{
	HomeViewModel viewModel;
	public HomePage(HomeViewModel viewModel)
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