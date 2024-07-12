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

    #region �׽�Ʈ �ڵ�
    // �׽�Ʈ �ڵ�
    private async void OnTestClicked(object sender, EventArgs e)
    {
        // AppShell�� ������ ��ȯ
        // ������ ��ȯ �̺�Ʈ
        if (Application.Current != null)
        {
            var appShell = new AppShell();
            await this.FadeTo(0, 100);
            Application.Current.MainPage = appShell;
            await appShell.FadeTo(1, 100);
        }
    }
    #endregion

    #region üũ�ڽ� ������ ���� ����
    // �� ����� ���� ���� �ҷ�����
    private async void LoadSavedUserData()
    {
        // �ڵ� �α��� üũ �� id, pw �ҷ�����
        if (AutoLoginCheck.IsChecked)
        {
            try
            {
                // ����� id, pw �ҷ�����
                UsernameEntry.Text = await SecureStorage.GetAsync("UserName");
                PasswordEntry.Text = await SecureStorage.GetAsync("UserPW");

                // �� ����� üũ�ڽ� üũ�Ǿ�����, id, pw�� ���� ��� üũ�ڽ� ����
                if (string.IsNullOrEmpty(UsernameEntry.Text) || string.IsNullOrEmpty(PasswordEntry.Text))
                {
                    SaveUserNameCheck.IsChecked = false;
                    AutoLoginCheck.IsChecked = false;
                    return;
                }

                // üũ�ڽ� üũ �� id, pw�� ���� ��� �ڵ� �α���
                await Login();
            }
            catch (Exception ex)
            {
                // SecureStorage ���� ó�� (��: ���� ����)
                await DisplayAlert(AppResources.error, AppResources.error + " : " + ex.Message, AppResources.ok);
            }
        }
        // ID ���常 �� ���  
        else if (SaveUserNameCheck.IsChecked)
        {
            try
            {
                UsernameEntry.Text = await SecureStorage.GetAsync("UserName");
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
                // üũ�ڽ� ��� ��üũ�� �� �����Ҷ� �����̸� ����
                SecureStorage.Remove("UserName");
            }
            catch (Exception ex)
            {
                // SecureStorage ���� ó�� (��: ���� ����)
                await DisplayAlert(AppResources.error, AppResources.error + " : " + ex.Message, AppResources.ok);
            }
        }
    }

    private async Task CheckBox_Check(string username, string password)
    {
        // �ڵ� �α��� üũ �� id, pw ����
        if (AutoLoginCheck.IsChecked)
        {
            try
            {
                await SecureStorage.SetAsync("UserName", username);
                await SecureStorage.SetAsync("UserPW", password);
            }
            catch (Exception ex)
            {
                // SecureStorage ���� ó�� (��: ���� ����)
                await DisplayAlert(AppResources.error, AppResources.error + " : " + ex.Message, AppResources.ok);
            }
        }
        // ���� id ���� üũ�� id�� ����
        else if (SaveUserNameCheck.IsChecked)
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
    }
    #endregion

    #region �α���
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
            LoadingOverlay.IsVisible = true; // �ε� �������� ǥ��LoadingOverlay.IsVisible = true; // �ε� �������� ǥ��

            ApiResponse apiResponse = await _loginviewmodel.LoginAsync(UsernameEntry.Text, PasswordEntry.Text);

            // �α��� ������
            if (apiResponse.StatusCode == 200)
            {
                // �α��� �����ÿ��� üũ�ڽ� ��� ����
                await CheckBox_Check(UsernameEntry.Text, PasswordEntry.Text);
                await SecureStorage.SetAsync("UserName", UsernameEntry.Text);

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
        catch (Exception ex)
        {
            await DisplayAlert(AppResources.error, AppResources.error + " : " + ex.Message, AppResources.ok);
        }
        finally
        {
            LoadingOverlay.IsVisible = false; // �ε� �������� �����
        }
    }
    #endregion
    
    #region ȸ������ �������� �̵�
    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        try
        {
            // ������ ��ȯ �̺�Ʈ
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

    #region â ����� üũ�ڽ� ����
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        // üũ�ڽ��� ���¸� �����մϴ�.
        Preferences.Set("SaveUserName", SaveUserNameCheck.IsChecked);
        Preferences.Set("AutoLogin", AutoLoginCheck.IsChecked);
    }
    #endregion
}