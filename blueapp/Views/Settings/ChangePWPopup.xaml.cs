using blackapi.Models;
using blueapp.Resources.Localization;
using blueapp.ViewModels;
using blueapp.Views.Splash;
using CommunityToolkit.Maui.Views;

namespace blueapp.Views.Settings;

public partial class ChangePWPopup : Popup
{
    private LoginViewModel _loginviewmodel;
    public ChangePWPopup(LoginViewModel loginviewmodel)
    {
        InitializeComponent();
        _loginviewmodel = loginviewmodel;
    }

    #region pw ����
    private async Task ChangePW()
    {
        try
        {
            LoadingOverlay.IsVisible = true; // �ε� �������� ǥ��LoadingOverlay.IsVisible = true; // �ε� �������� ǥ��

            ApiResponse apiResponse = await _loginviewmodel.ChangePWAsync(OldPasswordEntry.Text, NewPasswordEntry.Text, NewPasswordCheckEntry.Text);

            // ȸ��Ż�� ������
            if (apiResponse.StatusCode == 200)
            {
                // ������ ��ȯ �̺�Ʈ
                if (Application.Current != null)
                {
                    var loginPage = new LoginPage();
                    Application.Current.MainPage = loginPage;
                    await loginPage.FadeTo(1, 100);
                }

                // ȸ��Ż�� ���� �˾��˸�
                var alertPopup = new AlertPopup()
                {
                    MainText = AppResources.change_password + AppResources.success,
                    OkText = AppResources.ok
                };
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.ShowPopupAsync(alertPopup);
                }

                await CloseAsync();
            }
            else
            {
                maintext.Text = apiResponse.Message;
            }
        }
        catch (Exception ex)
        {
            maintext.Text = AppResources.error + " : " + ex.Message;
        }
        finally
        {
            LoadingOverlay.IsVisible = false; // �ε� �������� �����
        }
    }
    #endregion

    #region Ȯ��/���� ��ư
    // Ȯ�� ��ư�� Ŭ���� �� ����� �޼ҵ�
    private async void OnOkClicked(object sender, EventArgs e)
    {
        try
        {
            await ChangePW();
        }
        catch (Exception ex)
        {
            maintext.Text = AppResources.error + " : " + ex.Message;
        }
    }

    // ��� ��ư�� Ŭ���� �� ����� �޼ҵ�
    private async void OnCancleClicked(object sender, EventArgs e)
    {
        await CloseAsync();
    }
    #endregion
}