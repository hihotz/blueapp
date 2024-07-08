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
            // �α׾ƿ� ó��
            SecureStorage.Remove("UserPW");
            Preferences.Remove("AutoLogin");
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