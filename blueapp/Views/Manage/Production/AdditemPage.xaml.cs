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

    #region id ����
    private async Task Additem()
    {
        try
        {
            LoadingOverlay.IsVisible = true; // �ε� �������� ǥ��LoadingOverlay.IsVisible = true; // �ε� �������� ǥ��
            // name, pw ���� ����ִ��� Ȯ��
            if (string.IsNullOrEmpty(ProductName.Text) || string.IsNullOrEmpty(ProductCount.Text))
            {
                maintext.Text = AppResources.error + " : " + AppResources.text_is_empty;
                return;
            }
            // ȸ��Ż�� ������
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
            LoadingOverlay.IsVisible = false; // �ε� �������� �����
        }
    }
    #endregion

    #region Ȯ��/���� ��ư
    // Ȯ�� ��ư�� Ŭ���� �� ����� �޼ҵ�
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

    // ��� ��ư�� Ŭ���� �� ����� �޼ҵ�
    private async void OnCancleClicked(object sender, EventArgs e)
    {
        await CloseAsync();
    }
    #endregion
}