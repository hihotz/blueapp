using blueapp.Resources.Localization;
using blueapp.ViewModels;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace blueapp.Views.Splash;

public partial class SplashPage : ContentPage
{
    private SplashViewModel _splashviewmodel;
    public SplashPage()
    {
        InitializeComponent();
        _splashviewmodel = new SplashViewModel();
        InitializeApp();
    }

    // �� ���࿡ �ʿ��� ������ �����մϴ�.
    private async void InitializeApp()
    {
        try
        {
            // �� �ε� �� �޽��� ǥ��
            statusLabel.Text = AppResources.loading;
            await _splashviewmodel.Loading();

            statusLabel.Text = AppResources.server + " " + AppResources.check;
            await _splashviewmodel.ServerCheck();

            statusLabel.Text = AppResources.db + " " + AppResources.check;
            await _splashviewmodel.DBCheck();

            statusLabel.Text = AppResources.update + " " + AppResources.check;
            await _splashviewmodel.UpdateCheck();
        }
        catch (Exception ex)
        {
            await DisplayAlert(AppResources.error, ex.Message, AppResources.ok);
        }
        finally
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
    }
}