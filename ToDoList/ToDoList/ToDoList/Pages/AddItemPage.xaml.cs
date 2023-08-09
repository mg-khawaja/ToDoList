using ToDoList.ViewModels;

namespace ToDoList.Pages;

public partial class AddItemPage : ContentPage
{
	AddItemViewModel viewModel;

    public AddItemPage(AddItemViewModel viewModel)
	{
		this.viewModel = viewModel;
		BindingContext = viewModel;
		InitializeComponent();
	}
}