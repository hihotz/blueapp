using blackapi.Models;
using blueapp.Resources.Localization;
using blueapp.ViewModels;
using blueapp.Views.Splash;
using CommunityToolkit.Maui.Views;

namespace blueapp.Views.Settings;

public partial class DeleteIDPopup : Popup
{
    private LoginViewModel _loginviewmodel;
    internal DeleteIDPopup(LoginViewModel loginviewmodel)
	{
		InitializeComponent();
        _loginviewmodel = loginviewmodel;
        InitializeApp();
    }

    // 페이지 로드시 유저네임 불러오기
    private async void InitializeApp()
    {
        UsernameEntry.Text = await SecureStorage.GetAsync("UserName");
    }

    #region id 삭제
    private async Task DeleteID()
    {
        try
        {
            LoadingOverlay.IsVisible = true; // 로딩 오버레이 표시LoadingOverlay.IsVisible = true; // 로딩 오버레이 표시
            
            ApiResponse apiResponse = await _loginviewmodel.DeleteIDAsync(UsernameEntry.Text, PasswordEntry.Text);

            // 회원탈퇴 성공시
            if (apiResponse.StatusCode == 200)
            {
                // 페이지 전환 이벤트
                if (Application.Current != null)
                {
                    var loginPage = new LoginPage();
                    Application.Current.MainPage = loginPage;
                    await loginPage.FadeTo(1, 100);
                }
                
                // 회원탈퇴 성공 팝업알림
                var alertPopup = new AlertPopup()
                {
                    MainText = AppResources.delete_id + AppResources.success,
                    OkText = AppResources.ok
                };

                // 메인페이지로 이동
                if (Application.Current?.MainPage !=null)
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
            LoadingOverlay.IsVisible = false; // 로딩 오버레이 숨기기
        }
    }
    #endregion

    #region 확인/삭제 버튼
    // 확인 버튼이 클릭될 때 실행될 메소드
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

    // 취소 버튼이 클릭될 때 실행될 메소드
    private async void OnCancleClicked(object sender, EventArgs e)
    {
        await CloseAsync();
    }
    #endregion
}