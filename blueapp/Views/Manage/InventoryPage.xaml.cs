using blueapp.ViewModels;

namespace blueapp.Views.Manage;

public partial class InventoryPage : ContentPage
{
	ProductViewModel _viewModel;
    internal InventoryPage(ProductViewModel _productViewModel)
	{
		InitializeComponent();

		_viewModel = _productViewModel;
	}
}