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

    private void OnLoginClicked(object sender, EventArgs e)
    {
        DisplayAlert("hello", "world", "ok");
        //string username = UsernameEntry.Text;
        //string password = PasswordEntry.Text;
        //
        //bool loginSuccess = await _loginviewmodel.LoginAsync(username, password);
        //
        //if (loginSuccess)
        //{
        //    // 로그인 성공 메시지
        //    await Toast.Make(AppResources.login + AppResources.success, ToastDuration.Long).Show();
        //}
        //else
        //{
        //    // 로그인 실패 메시지 (MainViewModel에서 처리한 에러 메시지를 사용합니다.)
        //    await Toast.Make(AppResources.login + AppResources.failed, ToastDuration.Long).Show();
        //}
    }

    private async void InitializeApp()
    {
        try
        {
            
        }
        catch (Exception ex)
        {
            await DisplayAlert(AppResources.error, ex.Message, AppResources.ok);
        }
        finally
        {
            // 메인 페이지로 전환
            if (Application.Current != null)
            {
                Application.Current.MainPage = new AppShell();
            }
        }
    }
}