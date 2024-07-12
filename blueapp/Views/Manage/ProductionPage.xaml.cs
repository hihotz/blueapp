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
}