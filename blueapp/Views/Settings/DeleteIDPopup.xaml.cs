using blackapi.Models;
using blueapp.Resources.Localization;
using blueapp.ViewModels;
using blueapp.Views.Splash;
using CommunityToolkit.Maui.Views;

namespace blueapp.Views.Settings;

public partial class DeleteIDPopup : Popup
{
    private LoginViewModel _loginviewmodel;
    public DeleteIDPopup(LoginViewModel loginviewmodel)
	{
		InitializeComponent();
        _loginviewmodel = loginviewmodel;
        InitializeApp();
    }

    // ������ �ε�� �������� �ҷ�����
    private async void InitializeApp()
    {
        UsernameEntry.Text = await SecureStorage.GetAsync("UserName");
    }

    #region id ����
    private async Task DeleteID()
    {
        try
        {
            ApiResponse apiResponse = await _loginviewmodel.DeleteIDAsync(UsernameEntry.Text, PasswordEntry.Text);

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
                await CloseAsync();
            }
            else
            {
                maintext.Text = AppResources.error + " : " + apiResponse.Message;
            }
        }
        catch (Exception ex)
        {
            maintext.Text = AppResources.error + " : " + ex.Message;
        }
    }
    #endregion

    #region Ȯ��/���� ��ư
    // Ȯ�� ��ư�� Ŭ���� �� ����� �޼ҵ�
    private async void OnOkClicked(object sender, EventArgs e)
    {
        try
        {
            await DeleteID();
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