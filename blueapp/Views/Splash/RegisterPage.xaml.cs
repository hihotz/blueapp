using blackapi.Models;
using blueapp.Resources.Localization;
using blueapp.ViewModels;
using CommunityToolkit.Maui.Views;

namespace blueapp.Views.Splash;

public partial class RegisterPage : ContentPage
{
    private LoginViewModel _loginviewmodel;

    public RegisterPage(LoginViewModel loginviewmodel)
	{
		InitializeComponent();
        _loginviewmodel = loginviewmodel;
    }

    #region 버튼 클릭
    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        try
        {
            LoadingOverlay.IsVisible = true; // 로딩 오버레이 표시
            await Register();
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

    private async void OnGoBackClicked(object sender, EventArgs e)
    {
        await GoBack();
    }
    #endregion

    #region 회원가입
    private async Task Register()
    {
        try
        {
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;

            ApiResponse apiResponse = await _loginviewmodel.RegisterAsync(username, password);

            // 로그인 성공시
            if (apiResponse.StatusCode == 200)
            {
                await DisplayAlert(AppResources.register + " " + AppResources.success, apiResponse.Message, AppResources.ok);
                // 페이지 전환 이벤트
                await GoBack();
            }
            else
            {
                // 로그인 실패시 DisplayAlert로 사용자에게 알림 표시 : 로그인 실패, 에러메시지, 확인
                await DisplayAlert(AppResources.register + " " + AppResources.failed, apiResponse.Message, AppResources.ok);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert(AppResources.error, AppResources.error + " : " + ex.Message, AppResources.ok);
        }
    }
    #endregion

    #region 페이지 전환
    private async Task GoBack()
    {
        try
        {
            // 페이지 전환 이벤트
            if (Application.Current != null)
            {
                //var loginPage = new LoginPage();
                await this.FadeTo(0, 100);
                Application.Current.MainPage = LoginPage.Instance;
                await LoginPage.Instance.FadeTo(1, 100);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert(AppResources.error, AppResources.error + " : " + ex.Message, AppResources.ok);
        }
    }
    #endregion
}