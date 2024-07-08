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
            // ������ ��ȯ �̺�Ʈ
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
            // �α��� ���н� DisplayAlert�� ����ڿ��� �˸� ǥ�� : �α��� ����, �����޽���, Ȯ��
            await DisplayAlert(AppResources.login + " ", apiResponse.Message, AppResources.ok);
        }
    }
}