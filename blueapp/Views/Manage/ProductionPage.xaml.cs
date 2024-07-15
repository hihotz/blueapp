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
        InitializeLayout();
    }

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

    #region ��ư ���
    private void AddProduction(object sender, EventArgs e)
    {

    }
    #endregion
}