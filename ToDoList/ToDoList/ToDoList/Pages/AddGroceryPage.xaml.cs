using ToDoList.ViewModels;

namespace ToDoList.Pages;

public partial class AddGroceryPage : ContentPage
{
    AddGroceryViewModel viewModel;

    public AddGroceryPage(AddGroceryViewModel viewModel)
	{
		this.viewModel = viewModel;
		BindingContext = viewModel;
		InitializeComponent();
	}
}