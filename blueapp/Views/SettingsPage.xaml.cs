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
        // ��ũ��带 �����ϱ� ���� BindingContext ���
        this.BindingContext = _settingviewmodel;
        InitializeApp();
        InitializeLayout();
    }
    public async void InitializeApp()
    {
        UserName.Text = await SecureStorage.GetAsync("UserName");
    }

    #region ���̾ƿ� ����
    // �ʱ� ���̾ƿ� ���� �ε�
    private void InitializeLayout()
    {
        double width = this.Width;
        double height = this.Height;
        OnSizeAllocated(width, height);
    }

    // â ũ�� ������ ����� �°� ����
    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        // ���� ��� ���̾ƿ�
        if (width > height)
        {
            // ����
            if (MainGrid.RowDefinitions.Count > 2)
            {
                MainGrid.RowDefinitions.RemoveAt(2); // �� ��°, �� ��° RowDefinition ����
            }

            // �¿�
            if (MainGrid.ColumnDefinitions.Count < 2)
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); // �� ��° ColumnDefinition �߰�

            Grid.SetRow(LeftTopFrame, 0); // ���� ��� Frame
            Grid.SetColumn(LeftTopFrame, 0);

            Grid.SetRow(RightTopGrid, 0); // ���� ��� Grid
            Grid.SetColumn(RightTopGrid, 1);

            Grid.SetRow(LeftBottomGrid, 1); // ���� �ϴ� Grid
            Grid.SetColumn(LeftBottomGrid, 0);

            Grid.SetRow(RightBottomGrid, 1); // ���� �ϴ� Grid
            Grid.SetColumn(RightBottomGrid, 1);
        }
        // ���� ��� ���̾ƿ�
        else
        {
            // ����
            if (MainGrid.RowDefinitions.Count <= 2)
            {
                MainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star }); // �� ��° RowDefinition �߰�
                MainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star }); // �� ��° RowDefinition �߰�
            }

            // �¿�
            if (MainGrid.ColumnDefinitions.Count >= 2)
                MainGrid.ColumnDefinitions.RemoveAt(1); // �� ��° ColumnDefinition ����

            Grid.SetRow(LeftTopFrame, 0); // ��� Frame
            Grid.SetColumn(LeftTopFrame, 0);

            Grid.SetRow(RightTopGrid, 1); // �� ��° Row�� ���� ��� Grid
            Grid.SetColumn(RightTopGrid, 0);

            Grid.SetRow(LeftBottomGrid, 2); // �� ��° Row�� ���� �ϴ� Grid
            Grid.SetColumn(LeftBottomGrid, 0);

            Grid.SetRow(RightBottomGrid, 3); // �� ��° Row�� ���� �ϴ� Grid
            Grid.SetColumn(RightBottomGrid, 0);
        }
    }
    #endregion

    #region ȸ������ ����(��й�ȣ����, ȸ��Ż��, �α׾ƿ�)
    // ��й�ȣ����
    private async void OnChangePasswordClicked(object sender, EventArgs e)
    {
        // ��й�ȣ ���� �˾� ȣ��
        var changePWPopup = new ChangePWPopup(_loginviewmodel);
        await this.ShowPopupAsync(changePWPopup);
    }

    // ȸ��Ż��
    private async void OnDeleteIDClicked(object sender, EventArgs e)
    {
        // ȸ��Ż�� �˾� ȣ��
        var deleteIDPopup = new DeleteIDPopup(_loginviewmodel);
        await this.ShowPopupAsync(deleteIDPopup);
    }

    // �α׾ƿ�
    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        try
        {
            // �α׾ƿ� ó��
            _loginviewmodel.Logout();
            // ������ ��ȯ �̺�Ʈ
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

    // ��ũ��� ��� 
    private void OnDarkModeTouchGestureCompleted(object sender, EventArgs e)
    {
        // Switch�� ���� ���¸� �����Ͽ� ��� ���¸� �����մϴ�.
        DarkModeToggle.IsToggled = !DarkModeToggle.IsToggled;
    }

    private async void OnInfoClicked(object sender, EventArgs e)
    {
        // ���� �������� �̵�
        var infoPopup = new InfoPopup();
        await this.ShowPopupAsync(infoPopup);
    }
}