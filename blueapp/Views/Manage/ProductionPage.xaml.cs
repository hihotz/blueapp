using blueapp.Data;
using blueapp.ViewModels;

namespace blueapp.Views.Manage;

public partial class ProductionPage : ContentPage
{
    ProductViewModel _viewModel;

    public ProductionPage(ProductViewModel _productViewModel)
    {
        InitializeComponent();
        _viewModel = _productViewModel;
        BindingContext = _viewModel;
        InitializeLayout();
    }

    #region 페이지 로드시
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadProductions();
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

            Grid.SetRow(LeftGrid, 0); // 좌측 Frame
            Grid.SetColumn(LeftGrid, 0);

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

            Grid.SetRow(LeftGrid, 0); // 상단 Frame
            Grid.SetColumn(LeftGrid, 0);

            Grid.SetRow(RightFrame, 1); // 하단 Frame
            Grid.SetColumn(RightFrame, 0);
        }
    }
    #endregion

    #region 버튼 기능
    private void AddProduction(object sender, EventArgs e)
    {
        _viewModel.AddProductionCommand.Execute(null);
    }
    #endregion
}