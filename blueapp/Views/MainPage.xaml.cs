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

    // ������ 
    private ProductionPage _productionPage;
    private InventoryPage _inventoryPage;
    private QualityPage _qualityPage;

    public MainPage()
    {
        InitializeComponent();
        _databaseService = new DatabaseService();
        _viewModel = new GraphViewModel();
        _productViewModel = new ProductViewModel();

        // ������
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

    #region �׷��� ��ũ�Ѻ� �ڵ���ũ��
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // �׷��� ��ũ�� �並 ���� ���������� ��ũ��
        this.Dispatcher.Dispatch(async () =>
        {
            await graphicsScrollView.ScrollToAsync(graphicsScrollView.ContentSize.Width, 0, true);
        });
    }
    #endregion

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
            if (MainGrid.RowDefinitions.Count == 2)
                MainGrid.RowDefinitions.RemoveAt(1); // �� ��° RowDefinition ����

            if (MainGrid.ColumnDefinitions.Count < 2)
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); // �� ��° RowDefinition �߰�

            Grid.SetRow(LeftFrame, 0); // ���� Frame
            Grid.SetColumn(LeftFrame, 0);

            Grid.SetRow(RightFrame, 0); // ���� Frame
            Grid.SetColumn(RightFrame, 1);
        }
        // ���� ��� ���̾ƿ�
        else
        {
            if (MainGrid.RowDefinitions.Count < 2)
                MainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // �� ��° RowDefinition �߰�
            
            if (MainGrid.ColumnDefinitions.Count == 2)
                MainGrid.ColumnDefinitions.RemoveAt(1); // �� ��° RowDefinition ����

            Grid.SetRow(LeftFrame, 0); // ��� Frame
            Grid.SetColumn(LeftFrame, 0);

            Grid.SetRow(RightFrame, 1); // �ϴ� Frame
            Grid.SetColumn(RightFrame, 0);
        }
    }
    #endregion

    #region ������ �̵�
    // ���� ���� ������
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

    // ��� ���� ������
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
    
    // ǰ�� ���� ������
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

    #region �� �����
    private async void OnRatingClicked(object sender, EventArgs e)
    {
        try
        {
            await RatingRecord();
            // �׷��� ��ũ�� �並 ���� ���������� ��ũ��
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
            // 0~100 ���� ������ Ȯ��
            if (IsNumberInRange(Rate.Text))
            {
                // �� ��� �����
                var record = new OperationRecord
                {
                    Rate = int.Parse(Rate.Text),
                };
                await _databaseService.AddRecordAsync(record);

                // �� �α� �����
                await _databaseService.AddAppLogAsync(new AppLog
                {
                    UserName = await SecureStorage.GetAsync("UserName"),
                    Message = AppResources.rate + AppResources.record + AppResources.success + " : " + record,
                    Timestamp = DateTime.Now,
                    Success = "Success"
                });

                // db ���� �� �ؽ�Ʈ �ʱ�ȭ
                Rate.Text = "";
                await DisplayAlert(AppResources.error, AppResources.success, AppResources.ok);

                // �׷��� �����
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

    // 0���� 100 ���̰� Ȯ��
    private bool IsNumberInRange(string input)
    {
        // ���ڿ��� ���ڷ� ��ȯ �������� Ȯ��
        if (int.TryParse(input, out int number))
        {
            // ���ڰ� 0���� 100 �������� Ȯ��
            if (number >= 0 && number <= 100)
            {
                return true;
            }
        }
        // ��ȯ �Ұ��ϰų� ������ ����� false ��ȯ
        return false;
    }
    #endregion

    #region ���� ����/���� ��ư
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