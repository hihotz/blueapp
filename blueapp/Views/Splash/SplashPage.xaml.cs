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

    // 앱 실행에 필요한 동작을 수행합니다.
    private async void InitializeApp()
    {
        try
        {
            await _splashviewmodel.Loading();
            // 메인 페이지로 전환
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