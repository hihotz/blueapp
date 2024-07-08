using blackapi.Models;
using blueapp.Resources.Localization;
using blueapp.ViewModels;
using CommunityToolkit.Maui.Alerts;

namespace blueapp.Views.Splash;

public partial class LoginPage : ContentPage
{
    private LoginViewModel _loginviewmodel;

    public LoginPage()
	{
		InitializeComponent();
        _loginviewmodel = new LoginViewModel();
        SaveUserNameCheck.IsChecked = Preferences.Get("SaveUserName", false);
        AutoLoginCheck.IsChecked = Preferences.Get("AutoLogin", false);
        LoadSavedUserName();
    }

    #region 체크박스 유무에 따른 동작
    private async void LoadSavedUserName()
    {
        if (SaveUserNameCheck.IsChecked)
        {
            try
            {
                var savedUserName = await SecureStorage.GetAsync("UserName");
                if (savedUserName != null)
                {
                    UsernameEntry.Text = savedUserName;
                }
            }
            catch (Exception ex)
            {
                // SecureStorage 예외 처리 (예: 권한 문제)
                await DisplayAlert(AppResources.error, AppResources.error + " : " + ex.Message, AppResources.ok);
            }
        }
        else
        {
            try
            {
                SecureStorage.Remove("UserName");
            }
            catch (Exception ex)
            {
                // SecureStorage 예외 처리 (예: 권한 문제)
                await DisplayAlert(AppResources.error, AppResources.error + " : " + ex.Message, AppResources.ok);
            }
        }
    }
    #endregion

    #region 로그인
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string username = UsernameEntry.Text;
        string password = PasswordEntry.Text;

        ApiResponse apiResponse = await _loginviewmodel.LoginAsync(username, password);

        if (apiResponse.StatusCode == 200)
        {
            // 체크박스에 따른 로그인 정보 저장
            if (SaveUserNameCheck.IsChecked)
            {
                try
                {
                    await SecureStorage.SetAsync("UserName", username);
                }
                catch (Exception ex)
                {
                    // SecureStorage 예외 처리 (예: 권한 문제)
                    await DisplayAlert(AppResources.error, AppResources.error + " : " + ex.Message, AppResources.ok);
                }
            }

            // 페이지 전환 이벤트
            if (Application.Current != null)
            {
                var appShell = new AppShell();
                await this.FadeTo(0, 100);
                Application.Current.MainPage = appShell;
                await appShell.FadeTo(1, 100);
            }
        }
        else
        {
            // 로그인 실패시 DisplayAlert로 사용자에게 알림 표시 : 로그인 실패, 에러메시지, 확인
            await DisplayAlert(AppResources.login + " " + AppResources.failed, apiResponse.Message, AppResources.ok);
        }
    }
    #endregion

    #region 회원가입
    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        // // 페이지 전환 이벤트
        // if (Application.Current != null)
        // {
        //     var registerPage = new RegisterPage();
        //     await this.FadeTo(0, 100);
        //     Application.Current.MainPage = registerPage;
        //     await registerPage.FadeTo(1, 100);
        // }
        await Toast.Make("asdf").Show();
    }
    #endregion

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        // 체크박스의 상태를 저장합니다.
        Preferences.Set("SaveUserName", SaveUserNameCheck.IsChecked);
        Preferences.Set("AutoLogin", AutoLoginCheck.IsChecked);
    }
}