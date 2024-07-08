using blueapp.Resources.Localization;
using blueapp.Views.Splash;

namespace blueapp.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
    }

    private async void OnLooutClicked(object sender, EventArgs e)
    {
        try
        {
            // 로그아웃 처리
            SecureStorage.Remove("UserPW");
            Preferences.Remove("AutoLogin");
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