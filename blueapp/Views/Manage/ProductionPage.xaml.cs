using blueapp.Data;
using blueapp.Models;
using blueapp.ViewModels;
using blueapp.Views.Manage.Production;
using CommunityToolkit.Maui.Views;

namespace blueapp.Views.Manage;

public partial class ProductionPage : ContentPage
{
    ProductViewModel _viewModel;

    internal ProductionPage(ProductViewModel _productViewModel)
    {
        InitializeComponent();
        _viewModel = _productViewModel;
        BindingContext = _viewModel;
        InitializeApp();
        InitializeLayout();
    }

    #region ������ �ε��
    private async void InitializeApp()
    {
        await _viewModel.LoadProductions();
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
            if (MainGrid.RowDefinitions.Count >= 3)
                MainGrid.RowDefinitions.RemoveAt(1); // �� ��° RowDefinition ����
        
            if (MainGrid.ColumnDefinitions.Count == 1)
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); // �� ��° RowDefinition �߰�
        
            
            Grid.SetRow(LeftTopGrid, 0); // ���� ���
            Grid.SetColumn(LeftTopGrid, 0);
            
            Grid.SetRow(LeftFrame, 1); // ���� Frame
            Grid.SetColumn(LeftFrame, 0);
            
            Grid.SetRow(RightFrame, 1); // ���� Frame
            Grid.SetColumn(RightFrame, 1);
        }
        // ���� ��� ���̾ƿ�
        else
        {
            if (MainGrid.RowDefinitions.Count < 3)
                MainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star }); // �� ��° RowDefinition �߰�
            
            if (MainGrid.ColumnDefinitions.Count == 2)
                MainGrid.ColumnDefinitions.RemoveAt(1); // �� ��° RowDefinition ����
            
            Grid.SetRow(LeftTopGrid, 0); // ���� ���
            Grid.SetColumn(LeftTopGrid, 0);
            
            Grid.SetRow(LeftFrame, 1); // ��� Frame
            Grid.SetColumn(LeftFrame, 0);
            
            Grid.SetRow(RightFrame, 2); // �ϴ� Frame
            Grid.SetColumn(RightFrame, 0);
        }
    }
    #endregion

    #region �÷��Ǻ� ��� Ŭ��
    private void OnShowClicked(object sender, EventArgs e)
    {
        var frame = sender as Frame;
        var label = frame?.Content as Label;
        if (label != null)
        {
            var item = label.BindingContext as Product_Production;
            if (item != null)
            {
                DisplayAlert("Item Clicked", $"ID: {item.Id}", "OK");
            }
        }
    }
    #endregion

    #region ��ư ���
    private async void AddProduction(object sender, EventArgs e)
    {
        // ��й�ȣ ���� �˾� ȣ��
        var AdditemPopup = new AdditemPage(_viewModel);
        await this.ShowPopupAsync(AdditemPopup);
    }
    #endregion
}