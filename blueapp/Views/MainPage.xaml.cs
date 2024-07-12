using blueapp.Data;
using blueapp.Models;
using blueapp.Resources.Localization;
using blueapp.ViewModels;
using blueapp.Views.Manage;
using CommunityToolkit.Maui.Alerts;
using Microsoft.Maui.Graphics.Text;
using SQLite;
using System.Collections.ObjectModel;

namespace blueapp.Views;

public partial class MainPage : ContentPage
{
    private DatabaseService _databaseService;
    private GraphViewModel _viewModel;
    public ObservableCollection<string> LogItems { get; set; }

    public MainPage()
    {
        InitializeComponent();
        _databaseService = new DatabaseService();
        _viewModel = new GraphViewModel();
        this.BindingContext = _viewModel;
        LogItems = new ObservableCollection<string>();
        LogList.ItemsSource = LogItems;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // 첫 페이지 로드 
        await _viewModel.UpdateGraphRecords();

        // 그래프 스크롤 뷰를 가장 오른쪽으로 스크롤
        this.Dispatcher.Dispatch(async () =>
        {
            await graphicsScrollView.ScrollToAsync(graphicsScrollView.ContentSize.Width, 0, true);
        });
    }

    #region 프로젝트 구조도 
    // MainPage.xaml: 대시보드 및 기본 UI
    // ProductionPage.xaml: 생산 관리 UI
    // InventoryPage.xaml: 재고 관리 UI
    // QualityPage.xaml: 품질 관리 UI

    #endregion

    #region 페이지 이동
    // 생산 관리 페이지
    private async void OnProductionManagementClicked(object sender, EventArgs e)
    {
        // 페이지 이동 
        await Navigation.PushAsync(new ProductionPage());
    }

    // 재고 관리 페이지
    private async void OnInventoryManagementClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new InventoryPage());
    }

    // 품질 관리 페이지
    private async void OnQualityManagementClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new QualityPage());
    }
    #endregion

    #region 평가 남기기
    private async void OnRatingClicked(object sender, EventArgs e)
    {
        try
        {
            await RatingRecord();
        }
        catch (Exception ex)
        {
            await DisplayAlert(AppResources.error, AppResources.error + " : " + ex.Message, AppResources.ok);
        }
    }

    private async Task RatingRecord()
    {
        try
        {
            // 0~100 사이 값인지 확인
            if (IsNumberInRange(Rate.Text))
            {
                var record = new OperationRecord
                {
                    Rate = int.Parse(Rate.Text),
                };

                await _databaseService.AddRecordAsync(record);

                await _databaseService.AddAppLogAsync(new AppLog
                {
                    UserName = await SecureStorage.GetAsync("UserName"),
                    Message = AppResources.rate + AppResources.record + AppResources.success + " : " + record,
                    Timestamp = DateTime.Now,
                    Success = "Success"
                });
                Rate.Text = "";
                await DisplayAlert(AppResources.error, AppResources.success, AppResources.ok);
                
            }
            else
            {
                await DisplayAlert(AppResources.error, AppResources.enter_0_to_100, AppResources.ok);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert(AppResources.error, AppResources.error + " : " + ex.Message, AppResources.ok);
        }
    }

    // 0부터 100 사이값 확인
    private bool IsNumberInRange(string input)
    {
        // 문자열이 숫자로 변환 가능한지 확인
        if (int.TryParse(input, out int number))
        {
            // 숫자가 0에서 100 사이인지 확인
            if (number >= 0 && number <= 100)
            {
                return true;
            }
        }
        // 변환 불가하거나 범위를 벗어나면 false 반환
        return false;
    }
    #endregion

    #region 하단 로그 생성
    private void OnStartLogClicked(object sender, EventArgs e)
    {
        LogItems.Insert(0, "Application is start : " + DateTime.Now.ToString());
    }

    private void OnStopLogClicked(object sender, EventArgs e)
    {
        LogItems.Insert(0, "Production stopped at " + DateTime.Now.ToString());
    }
    #endregion
}