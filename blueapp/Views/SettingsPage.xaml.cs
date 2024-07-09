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
        InitializeApp();
        _loginviewmodel = new LoginViewModel();
        _settingviewmodel = new SettingViewModel(); 
        // 다크모드를 구현하기 위해 BindingContext 사용
        this.BindingContext = _settingviewmodel;
        
    }
    public async void InitializeApp()
    {
        UserName.Text = await SecureStorage.GetAsync("UserName");
    }

    #region 회원정보 관련(비밀번호변경, 회원탈퇴, 로그아웃)
    // 비밀번호변경
    private void OnChangePasswordClicked(object sender, EventArgs e)
    {
        // 비밀번호 변경 페이지로 이동
        // var changePasswordPopup = new ChangePasswordPopup();
        // await this.ShowPopupAsync(changePasswordPopup);
    }

    // 회원탈퇴
    private async void OnDeleteIDClicked(object sender, EventArgs e)
    {
        // 회원탈퇴 팝업 호출
        var deleteIDPopup = new DeleteIDPopup(_loginviewmodel);
        await this.ShowPopupAsync(deleteIDPopup);
    }

    // 로그아웃
    private async void OnLogoutClicked(object sender, EventArgs e)
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
    #endregion

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