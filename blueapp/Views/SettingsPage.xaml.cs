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
            // 로그아웃 처리
            _loginviewmodel.Logout();
            // 페이지 전환 이벤트
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