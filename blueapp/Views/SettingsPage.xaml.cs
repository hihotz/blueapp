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

        // 다크모드를 구현하기 위해 BindingContext 사용
        this.BindingContext = _settingviewmodel;
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

    // 다크모드 토글 
    private void OnDarkModeTouchGestureCompleted(object sender, EventArgs e)
    {
        // Switch의 현재 상태를 반전하여 토글 상태를 변경합니다.
        DarkModeToggle.IsToggled = !DarkModeToggle.IsToggled;
    }

    private async void OnInfoClicked(object sender, EventArgs e)
    {
        // 정보 페이지로 이동
        var infoPopup = new InfoPopup();
        await this.ShowPopupAsync(infoPopup);
    }
}