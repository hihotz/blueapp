using blackapi.Models;
using blueapp.Resources.Localization;
using blueapp.ViewModels;
using blueapp.Views.Splash;
using CommunityToolkit.Maui.Views;

namespace blueapp.Views.Manage.Production;

public partial class AdditemPage : Popup
{
    private ProductViewModel _productviewmodel;

    public AdditemPage(ProductViewModel productviewmodel)
    {
        InitializeComponent();
        _productviewmodel = productviewmodel;
    }

    #region id 삭제
    private async Task Additem()
    {
        try
        {
            LoadingOverlay.IsVisible = true; // 로딩 오버레이 표시LoadingOverlay.IsVisible = true; // 로딩 오버레이 표시
            // name, pw 값이 비어있는지 확인
            if (string.IsNullOrEmpty(ProductName.Text) || string.IsNullOrEmpty(ProductCount.Text))
            {
                maintext.Text = AppResources.error + " : " + AppResources.text_is_empty;
                return;
            }
            // 회원탈퇴 성공시
            if (await _productviewmodel.AddProduction(ProductName.Text, int.Parse(ProductCount.Text)))
            {
                await CloseAsync();
            }
            else
            {
                maintext.Text = AppResources.error;
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
            await Additem();
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