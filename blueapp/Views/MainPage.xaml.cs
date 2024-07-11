using CommunityToolkit.Maui.Alerts;
using System.Collections.ObjectModel;

namespace blueapp.Views;

public partial class MainPage : ContentPage
{
    public ObservableCollection<string> LogItems { get; set; }

    public MainPage()
	{
		InitializeComponent();
        LogItems = new ObservableCollection<string>();
        LogList.ItemsSource = LogItems;
    }

    #region ������Ʈ ������ 
    // MainPage.xaml: ��ú��� �� �⺻ UI
    // ProductionPage.xaml: ���� ���� UI
    // InventoryPage.xaml: ��� ���� UI
    // QualityPage.xaml: ǰ�� ���� UI

    #endregion

    #region �� �ε�
    private void InitializeApp()
    {
        try
        {
            LogItems.Insert(0, "Application is start : " + DateTime.Now.ToString());
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", ex.Message, "OK");
        }
    }

    #endregion

    #region ������ �̵�
    // ���� ���� ������
    private async void OnProductionManagementClicked(object sender, EventArgs e)
    {
        // ������ �̵� 
        // await Navigation.PushAsync(new ProductionPage());
        await DisplayAlert("�˸�", "�غ����Դϴ�.", "Ȯ��");
    }

    // ��� ���� ������
    private async void OnInventoryManagementClicked(object sender, EventArgs e)
    {
        //await Navigation.PushAsync(new InventoryPage());
        await DisplayAlert("�˸�", "�غ����Դϴ�.", "Ȯ��");
    }

    // ǰ�� ���� ������
    private async void OnQualityManagementClicked(object sender, EventArgs e)
    {
        //await Navigation.PushAsync(new QualityPage());
        await DisplayAlert("�˸�", "�غ����Դϴ�.", "Ȯ��");
    }
    #endregion

    private void OnStartLogClicked(object sender, EventArgs e)
    {
        LogItems.Insert(0, "Application is start : " + DateTime.Now.ToString());
    }

    private void OnStopLogClicked(object sender, EventArgs e)
    {
        LogItems.Insert(0, "Production stopped at " + DateTime.Now.ToString());
    }

}