using blackapi.Models;
using blueapp.Resources.Localization;
using blueapp.ViewModels;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using System.Xml.Linq;

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
        LoadSavedUserData();
    }

    #region 테스트 코드
    // 테스트 코드
    private async void OnTestClicked(object sender, EventArgs e)
    {
        // AppShell로 페이지 전환
        // 페이지 전환 이벤트
        if (Application.Current != null)
        {
            var appShell = new AppShell();
            await this.FadeTo(0, 100);
            Application.Current.MainPage = appShell;
            await appShell.FadeTo(1, 100);
        }
    }
    #endregion

    #region 체크박스 유무에 따른 동작
    // 앱 실행시 유저 정보 불러오기
    private async void LoadSavedUserData()
    {
        // 자동 로그인 체크 시 id, pw 불러오기
        if (AutoLoginCheck.IsChecked)
        {
            try
            {
                // 저장된 id, pw 불러오기
                UsernameEntry.Text = await SecureStorage.GetAsync("UserName");
                PasswordEntry.Text = await SecureStorage.GetAsync("UserPW");

                // 앱 실행시 체크박스 체크되었지만, id, pw가 없을 경우 체크박스 해제
                if (string.IsNullOrEmpty(UsernameEntry.Text) || string.IsNullOrEmpty(PasswordEntry.Text))
                {
                    SaveUserNameCheck.IsChecked = false;
                    AutoLoginCheck.IsChecked = false;
                    return;
                }

                // 체크박스 체크 및 id, pw가 있을 경우 자동 로그인
                await Login();
            }
            catch (Exception ex)
            {
                // SecureStorage 예외 처리 (예: 권한 문제)
                await DisplayAlert(AppResources.error, AppResources.error + " : " + ex.Message, AppResources.ok);
            }
        }
        // ID 저장만 한 경우  
        else if (SaveUserNameCheck.IsChecked)
        {
            try
            {
                UsernameEntry.Text = await SecureStorage.GetAsync("UserName");
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
                // 체크박스 모두 미체크시 앱 실행할때 유저이름 삭제
                SecureStorage.Remove("UserName");
            }
            catch (Exception ex)
            {
                // SecureStorage 예외 처리 (예: 권한 문제)
                await DisplayAlert(AppResources.error, AppResources.error + " : " + ex.Message, AppResources.ok);
            }
        }
    }

    private async Task CheckBox_Check(string username, string password)
    {
        // 자동 로그인 체크 시 id, pw 저장
        if (AutoLoginCheck.IsChecked)
        {
            try
            {
                await SecureStorage.SetAsync("UserName", username);
                await SecureStorage.SetAsync("UserPW", password);
            }
            catch (Exception ex)
            {
                // SecureStorage 예외 처리 (예: 권한 문제)
                await DisplayAlert(AppResources.error, AppResources.error + " : " + ex.Message, AppResources.ok);
            }
        }
        // 유저 id 저장 체크시 id만 저장
        else if (SaveUserNameCheck.IsChecked)
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
    }
    #endregion

    #region 로그인
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        try
        {
            await Login();
        }
        catch (Exception ex)
        {
            await DisplayAlert(AppResources.error, AppResources.error + " : " + ex.Message, AppResources.ok);
        }
    }

    private async Task Login()
    {
        try
        {
            LoadingOverlay.IsVisible = true; // 로딩 오버레이 표시LoadingOverlay.IsVisible = true; // 로딩 오버레이 표시

            ApiResponse apiResponse = await _loginviewmodel.LoginAsync(UsernameEntry.Text, PasswordEntry.Text);

            // 로그인 성공시
            if (apiResponse.StatusCode == 200)
            {
                // 로그인 성공시에만 체크박스 기능 동작
                await CheckBox_Check(UsernameEntry.Text, PasswordEntry.Text);
                await SecureStorage.SetAsync("UserName", UsernameEntry.Text);

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
        catch (Exception ex)
        {
            await DisplayAlert(AppResources.error, AppResources.error + " : " + ex.Message, AppResources.ok);
        }
        finally
        {
            LoadingOverlay.IsVisible = false; // 로딩 오버레이 숨기기
        }
    }
    #endregion
    
    #region 회원가입 페이지로 이동
    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        try
        {
            // 페이지 전환 이벤트
            if (Application.Current != null)
            {
                var registerPage = new RegisterPage(_loginviewmodel);
                await this.FadeTo(0, 100);
                Application.Current.MainPage = registerPage;
                await registerPage.FadeTo(1, 100);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert(AppResources.error, AppResources.error + " : " + ex.Message, AppResources.ok);
        }
    }
    #endregion

    #region 창 종료시 체크박스 저장
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        // 체크박스의 상태를 저장합니다.
        Preferences.Set("SaveUserName", SaveUserNameCheck.IsChecked);
        Preferences.Set("AutoLogin", AutoLoginCheck.IsChecked);
    }
    #endregion
}