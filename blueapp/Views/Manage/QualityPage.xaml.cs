using blueapp.ViewModels;

namespace blueapp.Views.Manage;

public partial class QualityPage : ContentPage
{
    ProductViewModel _viewModel;
    internal QualityPage(ProductViewModel _productViewModel)
    {
        InitializeComponent();

        _viewModel = _productViewModel;
    }
}