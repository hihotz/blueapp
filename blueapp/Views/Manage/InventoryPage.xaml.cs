using blueapp.ViewModels;

namespace blueapp.Views.Manage;

public partial class InventoryPage : ContentPage
{
	ProductViewModel _viewModel;
	public InventoryPage(ProductViewModel _productViewModel)
	{
		InitializeComponent();

		_viewModel = _productViewModel;
	}
}