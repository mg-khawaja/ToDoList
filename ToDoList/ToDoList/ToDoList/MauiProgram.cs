using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Compatibility.Hosting;
using Syncfusion.Maui.Core.Hosting;
using ToDoList.Pages;
using ToDoList.SQLiteDb;
using ToDoList.ViewModels;

namespace ToDoList;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCompatibility()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
        builder.Services.AddSingleton<GroceriesDatabase>();
        builder.Services.AddSingleton<HomePage, HomeViewModel>();
        builder.Services.AddSingleton<GroceriesPage, GroceriesViewModel>();
        builder.Services.AddTransient<GroceryPage, GroceryViewModel>();
        builder.Services.AddTransient<AddItemPage, AddItemViewModel>();
        builder.Services.AddTransient<AddGroceryPage, AddGroceryViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.ConfigureSyncfusionCore();
        return builder.Build();
	}
}
