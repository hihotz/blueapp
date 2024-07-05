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
        //    // �α��� ���� �޽���
        //    await Toast.Make(AppResources.login + AppResources.success, ToastDuration.Long).Show();
        //}
        //else
        //{
        //    // �α��� ���� �޽��� (MainViewModel���� ó���� ���� �޽����� ����մϴ�.)
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
            // ���� �������� ��ȯ
            if (Application.Current != null)
            {
                Application.Current.MainPage = new AppShell();
            }
        }
    }
}