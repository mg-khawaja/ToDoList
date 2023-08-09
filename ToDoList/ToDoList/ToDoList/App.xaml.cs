namespace ToDoList;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjYzMjU0M0AzMjMyMmUzMDJlMzBhTEticFMxT0ZaN2lhMm8rbnJIdVZ0Z0N0V05JaTdkTnd6OUc5TytwUlRvPQ==");
        MainPage = new AppShell();
    }
    protected override void OnStart()
    {
    }
}
