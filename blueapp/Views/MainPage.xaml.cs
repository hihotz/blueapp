using blueapp.Models;
using blueapp.Resources.Localization;
using blueapp.Service;
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
    private ProductViewModel _productViewModel;
    protected ObservableCollection<string> LogItems { get; set; }

    // 페이지 
    private ProductionPage _productionPage;
    private InventoryPage _inventoryPage;
    private QualityPage _qualityPage;

    public MainPage()
    {
        InitializeComponent();
        _databaseService = new DatabaseService();
        _viewModel = new GraphViewModel();
        _productViewModel = new ProductViewModel();

        // 페이지
        _productionPage = new ProductionPage(_productViewModel);
        _inventoryPage = new InventoryPage(_productViewModel);
        _qualityPage = new QualityPage(_productViewModel);

        this.BindingContext = _viewModel;
        LogItems = new ObservableCollection<string>();
        LogList.ItemsSource = LogItems;
        LogItems.Insert(0, "Application is start : " + DateTime.Now.ToString());
        _viewModel.IsRefreshing = true;
        InitializeLayout();
    }

    #region 그래프 스크롤뷰 자동스크롤
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // 그래프 스크롤 뷰를 가장 오른쪽으로 스크롤
        this.Dispatcher.Dispatch(async () =>
        {
            await graphicsScrollView.ScrollToAsync(graphicsScrollView.ContentSize.Width, 0, true);
        });
    }
    #endregion

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
            if (MainGrid.RowDefinitions.Count == 2)
                MainGrid.RowDefinitions.RemoveAt(1); // 두 번째 RowDefinition 제거

            if (MainGrid.ColumnDefinitions.Count < 2)
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); // 두 번째 RowDefinition 추가

            Grid.SetRow(LeftFrame, 0); // 좌측 Frame
            Grid.SetColumn(LeftFrame, 0);

            Grid.SetRow(RightFrame, 0); // 우측 Frame
            Grid.SetColumn(RightFrame, 1);
        }
        // 세로 모드 레이아웃
        else
        {
            if (MainGrid.RowDefinitions.Count < 2)
                MainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // 두 번째 RowDefinition 추가
            
            if (MainGrid.ColumnDefinitions.Count == 2)
                MainGrid.ColumnDefinitions.RemoveAt(1); // 두 번째 RowDefinition 제거

            Grid.SetRow(LeftFrame, 0); // 상단 Frame
            Grid.SetColumn(LeftFrame, 0);

            Grid.SetRow(RightFrame, 1); // 하단 Frame
            Grid.SetColumn(RightFrame, 0);
        }
    }
    #endregion

    #region 페이지 이동
    // 생산 관리 페이지
    private async void OnProductionManagementClicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(_productionPage);
        }
        catch (Exception ex)
        {
            await DisplayAlert(AppResources.error, AppResources.error + " : " + ex.Message, AppResources.ok);
        }
    }

    // 재고 관리 페이지
    private async void OnInventoryManagementClicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(_inventoryPage);
        }
        catch (Exception ex)
        {
            await DisplayAlert(AppResources.error, AppResources.error + " : " + ex.Message, AppResources.ok);
        }
    }
    
    // 품질 관리 페이지
    private async void OnQualityManagementClicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(_qualityPage);
        }
        catch (Exception ex)
        {
            await DisplayAlert(AppResources.error, AppResources.error + " : " + ex.Message, AppResources.ok);
        }
    }
    #endregion

    #region 평가 남기기
    private async void OnRatingClicked(object sender, EventArgs e)
    {
        try
        {
            await RatingRecord();
            // 그래프 스크롤 뷰를 가장 오른쪽으로 스크롤
            this.Dispatcher.Dispatch(async () =>
            {
                await graphicsScrollView.ScrollToAsync(graphicsScrollView.ContentSize.Width, 0, true);
            });
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
                // 평가 기록 남기기
                var record = new OperationRecord
                {
                    Rate = int.Parse(Rate.Text),
                };
                await _databaseService.AddRecordAsync(record);

                // 앱 로그 남기기
                await _databaseService.AddAppLogAsync(new AppLog
                {
                    UserName = await SecureStorage.GetAsync("UserName"),
                    Message = AppResources.rate + AppResources.record + AppResources.success + " : " + record,
                    Timestamp = DateTime.Now,
                    Success = "Success"
                });

                // db 저장 후 텍스트 초기화
                Rate.Text = "";
                await DisplayAlert(AppResources.error, AppResources.success, AppResources.ok);

                // 그래프 재생성
                await _viewModel.RefreshCommand.ExecuteAsync();
                //_viewModel.IsRefreshing = true;
                // _viewModel.DrawGraph();
                // await _viewModel.UpdateGraphRecords();
                //await _viewModel.RefreshGraph();
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

    #region 생산 시작/중지 버튼
    private void OnStartLogClicked(object sender, EventArgs e)
    {
        LogItems.Insert(0, AppResources.application_started + " : " + DateTime.Now.ToString());
    }

    private void OnStopLogClicked(object sender, EventArgs e)
    {
        LogItems.Insert(0, AppResources.application_stopped + " : " + DateTime.Now.ToString());
    }
    #endregion
}