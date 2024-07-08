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

    #region üũ�ڽ� ������ ���� ����
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
                // SecureStorage ���� ó�� (��: ���� ����)
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
                // SecureStorage ���� ó�� (��: ���� ����)
                await DisplayAlert(AppResources.error, AppResources.error + " : " + ex.Message, AppResources.ok);
            }
        }
    }
    #endregion

    #region �α���
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string username = UsernameEntry.Text;
        string password = PasswordEntry.Text;

        ApiResponse apiResponse = await _loginviewmodel.LoginAsync(username, password);

        if (apiResponse.StatusCode == 200)
        {
            // üũ�ڽ��� ���� �α��� ���� ����
            if (SaveUserNameCheck.IsChecked)
            {
                try
                {
                    await SecureStorage.SetAsync("UserName", username);
                }
                catch (Exception ex)
                {
                    // SecureStorage ���� ó�� (��: ���� ����)
                    await DisplayAlert(AppResources.error, AppResources.error + " : " + ex.Message, AppResources.ok);
                }
            }

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
            await DisplayAlert(AppResources.login + " " + AppResources.failed, apiResponse.Message, AppResources.ok);
        }
    }
    #endregion

    #region ȸ������
    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        // // ������ ��ȯ �̺�Ʈ
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
        // üũ�ڽ��� ���¸� �����մϴ�.
        Preferences.Set("SaveUserName", SaveUserNameCheck.IsChecked);
        Preferences.Set("AutoLogin", AutoLoginCheck.IsChecked);
    }
}