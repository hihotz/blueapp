using blueapp.Resources.Localization;
using blueapp.ViewModels;
using blueapp.Views.Splash;

namespace blueapp.Views;

public partial class SettingsPage : ContentPage
{
    LoginViewModel _loginviewmodel;
    public SettingsPage()
    {
        InitializeComponent();
        _loginviewmodel = new LoginViewModel();
    }

    private async void OnLooutClicked(object sender, EventArgs e)
    {
        try
        {
            // �α׾ƿ� ó��
            _loginviewmodel.Logout();
            // ������ ��ȯ �̺�Ʈ
            if (Application.Current != null)
            {
                var loginPage = new LoginPage();
                await this.FadeTo(0, 100);
                Application.Current.MainPage = loginPage;
                await loginPage.FadeTo(1, 100);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert(AppResources.error, ex.Message, AppResources.ok);
        }
    }
}