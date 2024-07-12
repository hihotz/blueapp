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
        // 첫 페이지 로드
        await _viewModel.LoadRecords(0);

        // 그래프 스크롤 뷰를 가장 오른쪽으로 스크롤
        this.Dispatcher.Dispatch(async () =>
        {
            await graphicsScrollView.ScrollToAsync(graphicsScrollView.ContentSize.Width, 0, true);
        });
    }
}