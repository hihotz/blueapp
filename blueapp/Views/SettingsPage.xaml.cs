using blueapp.Resources.Localization;
using blueapp.ViewModels;
using blueapp.Views.Settings;
using blueapp.Views.Splash;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;

namespace blueapp.Views;

public partial class SettingsPage : ContentPage
{
    LoginViewModel _loginviewmodel;
    SettingViewModel _settingviewmodel;
    public SettingsPage()
    {
        InitializeComponent();
        _loginviewmodel = new LoginViewModel();
        _settingviewmodel = new SettingViewModel();
        // 다크모드를 구현하기 위해 BindingContext 사용
        this.BindingContext = _settingviewmodel;
        InitializeApp();
        InitializeLayout();
    }
    public async void InitializeApp()
    {
        UserName.Text = await SecureStorage.GetAsync("UserName");
    }

    #region 레이아웃 변경
    // 초기 레이아웃 상태 로드
    private void InitializeLayout()
    {
        double width = this.Width;
        double height = this.Height;
        OnSizeAllocated(width, height);
    }

    // 창 크기 변동시 사이즈에 맞게 수정
    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        // 가로 모드 레이아웃
        if (width > height)
        {
            // 상하
            if (MainGrid.RowDefinitions.Count > 2)
            {
                MainGrid.RowDefinitions.RemoveAt(2); // 세 번째, 네 번째 RowDefinition 제거
            }

            // 좌우
            if (MainGrid.ColumnDefinitions.Count < 2)
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); // 두 번째 ColumnDefinition 추가

            Grid.SetRow(LeftTopFrame, 0); // 좌측 상단 Frame
            Grid.SetColumn(LeftTopFrame, 0);

            Grid.SetRow(RightTopGrid, 0); // 우측 상단 Grid
            Grid.SetColumn(RightTopGrid, 1);

            Grid.SetRow(LeftBottomGrid, 1); // 좌측 하단 Grid
            Grid.SetColumn(LeftBottomGrid, 0);

            Grid.SetRow(RightBottomGrid, 1); // 우측 하단 Grid
            Grid.SetColumn(RightBottomGrid, 1);
        }
        // 세로 모드 레이아웃
        else
        {
            // 상하
            if (MainGrid.RowDefinitions.Count <= 2)
            {
                MainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star }); // 세 번째 RowDefinition 추가
                MainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star }); // 네 번째 RowDefinition 추가
            }

            // 좌우
            if (MainGrid.ColumnDefinitions.Count >= 2)
                MainGrid.ColumnDefinitions.RemoveAt(1); // 두 번째 ColumnDefinition 제거

            Grid.SetRow(LeftTopFrame, 0); // 상단 Frame
            Grid.SetColumn(LeftTopFrame, 0);

            Grid.SetRow(RightTopGrid, 1); // 두 번째 Row에 우측 상단 Grid
            Grid.SetColumn(RightTopGrid, 0);

            Grid.SetRow(LeftBottomGrid, 2); // 세 번째 Row에 좌측 하단 Grid
            Grid.SetColumn(LeftBottomGrid, 0);

            Grid.SetRow(RightBottomGrid, 3); // 네 번째 Row에 우측 하단 Grid
            Grid.SetColumn(RightBottomGrid, 0);
        }
    }
    #endregion

    #region 회원정보 관련(비밀번호변경, 회원탈퇴, 로그아웃)
    // 비밀번호변경
    private async void OnChangePasswordClicked(object sender, EventArgs e)
    {
        // 비밀번호 변경 팝업 호출
        var changePWPopup = new ChangePWPopup(_loginviewmodel);
        await this.ShowPopupAsync(changePWPopup);
    }

    // 회원탈퇴
    private async void OnDeleteIDClicked(object sender, EventArgs e)
    {
        // 회원탈퇴 팝업 호출
        var deleteIDPopup = new DeleteIDPopup(_loginviewmodel);
        await this.ShowPopupAsync(deleteIDPopup);
    }

    // 로그아웃
    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        try
        {
            // 로그아웃 처리
            _loginviewmodel.Logout();
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
    #endregion

    // 다크모드 토글 
    private void OnDarkModeTouchGestureCompleted(object sender, EventArgs e)
    {
        // Switch의 현재 상태를 반전하여 토글 상태를 변경합니다.
        DarkModeToggle.IsToggled = !DarkModeToggle.IsToggled;
    }

    private async void OnInfoClicked(object sender, EventArgs e)
    {
        // 정보 페이지로 이동
        var infoPopup = new InfoPopup();
        await this.ShowPopupAsync(infoPopup);
    }
}