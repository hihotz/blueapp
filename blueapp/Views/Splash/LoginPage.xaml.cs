using blackapi.Models;
using blueapp.Resources.Localization;
using blueapp.ViewModels;

namespace blueapp.Views.Splash;

public partial class LoginPage : ContentPage
{
    private LoginViewModel _loginviewmodel;
    public LoginPage()
	{
		InitializeComponent();
        _loginviewmodel = new LoginViewModel();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string username = UsernameEntry.Text;
        string password = PasswordEntry.Text;

        ApiResponse apiResponse = await _loginviewmodel.LoginAsync(username, password);

        if (apiResponse.StatusCode == 200)
        {
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
            await DisplayAlert(AppResources.login + " ", apiResponse.Message, AppResources.ok);
        }
    }
}