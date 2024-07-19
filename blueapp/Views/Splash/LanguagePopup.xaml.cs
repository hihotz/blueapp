using blueapp.ViewModels;
using CommunityToolkit.Maui.Views;
namespace blueapp.Views.Splash;

public partial class LanguagePopup : Popup
{
    private LanguageViewModel _viewModel;
    public LanguagePopup()
    {
        InitializeComponent();
        _viewModel = new LanguageViewModel();
    }

    private async void OnKoreanSelected(object sender, EventArgs e)
    {
        _viewModel.Change("ko");
        
        await CloseAsync();
        if (Application.Current != null)
        {
            var loginPage = new LoginPage();
            Application.Current.MainPage = loginPage;
            await loginPage.FadeTo(1, 100);
        }
    }

    private async void OnEnglishSelected(object sender, EventArgs e)
    {
        _viewModel.Change("en");
        
        await CloseAsync();
        if (Application.Current != null)
        {
            var loginPage = new LoginPage();
            Application.Current.MainPage = loginPage;
            await loginPage.FadeTo(1, 100);
        }
    }
}
