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

            // 페이지 전환 이벤트
            if (Application.Current != null)
            {
                var loginPage = new LoginPage();
                await this.FadeTo(0, 100);
                Application.Current.MainPage = loginPage;
                await loginPage.FadeTo(1, 100); 
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert(AppResources.error, ex.Message, AppResources.ok);
        }
    }
}