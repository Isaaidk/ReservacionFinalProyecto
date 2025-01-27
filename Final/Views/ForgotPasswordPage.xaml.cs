namespace Final.Views;

public partial class ForgotPasswordPage : ContentPage
{
	public ForgotPasswordPage()
	{
		InitializeComponent();
	}

    private async void OnBackToLoginClicked(object sender, EventArgs e)
    {
        // Navega de vuelta a la página principal (LoginPage)
        await Navigation.PushAsync(new Views.LoginPage());
    }
}