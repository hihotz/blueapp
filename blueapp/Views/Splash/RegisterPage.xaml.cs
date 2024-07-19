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

    #region ��ư Ŭ��
    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        try
        {
            LoadingOverlay.IsVisible = true; // �ε� �������� ǥ��
            await Register();
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

    private async void OnGoBackClicked(object sender, EventArgs e)
    {
        await GoBack();
    }
    #endregion

    #region ȸ������
    private async Task Register()
    {
        try
        {
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;

            ApiResponse apiResponse = await _loginviewmodel.RegisterAsync(username, password);

            // �α��� ������
            if (apiResponse.StatusCode == 200)
            {
                await DisplayAlert(AppResources.register + " " + AppResources.success, apiResponse.Message, AppResources.ok);
                // ������ ��ȯ �̺�Ʈ
                await GoBack();
            }
            else
            {
                // �α��� ���н� DisplayAlert�� ����ڿ��� �˸� ǥ�� : �α��� ����, �����޽���, Ȯ��
                await DisplayAlert(AppResources.register + " " + AppResources.failed, apiResponse.Message, AppResources.ok);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert(AppResources.error, AppResources.error + " : " + ex.Message, AppResources.ok);
        }
    }
    #endregion

    #region ������ ��ȯ
    private async Task GoBack()
    {
        try
        {
            // ������ ��ȯ �̺�Ʈ
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