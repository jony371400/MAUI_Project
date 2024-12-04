namespace MauiApp1;

public partial class HomePage : ContentPage
{
    public Command NavigateCommand { get; }

    public HomePage()
	{
		InitializeComponent();

        NavigateCommand = new Command<string>(OnNavigate);
        BindingContext = this;
        InitPage();

    }

    private async void OnNavigate(string pageName)
    {
        await Shell.Current.GoToAsync(pageName);
    }

    async void InitPage()
	{
        await DisplayAlert("Login Success", "Welcome", "OK");
    }
}