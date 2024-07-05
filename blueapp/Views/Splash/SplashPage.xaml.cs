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
            await _splashviewmodel.Loading();
            // ���� �������� ��ȯ
            if (Application.Current != null)
            {
                Application.Current.MainPage = new LoginPage();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert(AppResources.error, ex.Message, AppResources.ok);
        }
    }
}