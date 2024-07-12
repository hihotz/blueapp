using blueapp.Data;
using blueapp.ViewModels;

namespace blueapp.Views.Manage;

public partial class ProductionPage : ContentPage
{
    private GraphViewModel _viewModel;
    public ProductionPage()
	{
		InitializeComponent();
        _viewModel = new GraphViewModel();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // ù ������ �ε�
        await _viewModel.LoadRecords(0);

        // �׷��� ��ũ�� �並 ���� ���������� ��ũ��
        this.Dispatcher.Dispatch(async () =>
        {
            await graphicsScrollView.ScrollToAsync(graphicsScrollView.ContentSize.Width, 0, true);
        });
    }
}