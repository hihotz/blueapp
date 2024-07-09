using blueapp.Resources.Localization;
using blueapp.ViewModels;
using blueapp.Views.Settings;
using blueapp.Views.Splash;
using CommunityToolkit.Maui.Views;

namespace blueapp.Views;

public partial class SettingsPage : ContentPage
{
    LoginViewModel _loginviewmodel;
    SettingViewModel _settingviewmodel;
    public SettingsPage()
    {
        InitializeComponent();
        _loginviewmodel = new LoginViewModel();
        _settingviewmodel = new SettingViewModel();

        // ��ũ��带 �����ϱ� ���� BindingContext ���
        this.BindingContext = _settingviewmodel;
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

    // ��ũ��� ��� 
    private void OnDarkModeTouchGestureCompleted(object sender, EventArgs e)
    {
        // Switch�� ���� ���¸� �����Ͽ� ��� ���¸� �����մϴ�.
        DarkModeToggle.IsToggled = !DarkModeToggle.IsToggled;
    }

    private async void OnInfoClicked(object sender, EventArgs e)
    {
        // ���� �������� �̵�
        var infoPopup = new InfoPopup();
        await this.ShowPopupAsync(infoPopup);
    }
}