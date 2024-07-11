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

    #region 프로젝트 구조도 
    // MainPage.xaml: 대시보드 및 기본 UI
    // ProductionPage.xaml: 생산 관리 UI
    // InventoryPage.xaml: 재고 관리 UI
    // QualityPage.xaml: 품질 관리 UI

    #endregion

    #region 앱 로딩
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

    #region 페이지 이동
    // 생산 관리 페이지
    private async void OnProductionManagementClicked(object sender, EventArgs e)
    {
        // 페이지 이동 
        // await Navigation.PushAsync(new ProductionPage());
        await DisplayAlert("알림", "준비중입니다.", "확인");
    }

    // 재고 관리 페이지
    private async void OnInventoryManagementClicked(object sender, EventArgs e)
    {
        //await Navigation.PushAsync(new InventoryPage());
        await DisplayAlert("알림", "준비중입니다.", "확인");
    }

    // 품질 관리 페이지
    private async void OnQualityManagementClicked(object sender, EventArgs e)
    {
        //await Navigation.PushAsync(new QualityPage());
        await DisplayAlert("알림", "준비중입니다.", "확인");
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