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
    }
}