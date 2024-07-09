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