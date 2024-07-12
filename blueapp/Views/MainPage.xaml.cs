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
        LogItems.Insert(0, "Application is start : " + DateTime.Now.ToString());

        CreateGraph();
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

    #region ������ �̵�
    // ���� ���� ������
    private async void OnProductionManagementClicked(object sender, EventArgs e)
    {
        // ������ �̵� 
        await Navigation.PushAsync(new ProductionPage());
    }

    // ��� ���� ������
    private async void OnInventoryManagementClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new InventoryPage());
    }

    // ǰ�� ���� ������
    private async void OnQualityManagementClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new QualityPage());
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
                CreateGraph();
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

    private async void CreateGraph()
    {
        await _viewModel.RefreshGraph();
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