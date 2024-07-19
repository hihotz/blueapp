using blueapp.ViewModels;
using CommunityToolkit.Maui.Views;
namespace blueapp.Views.Settings;

public partial class LanguagePopup : Popup
{
    private LanguageViewModel _viewModel;
    public LanguagePopup()
	{
		InitializeComponent();
        _viewModel = new LanguageViewModel();
    }

    private void OnKoreanSelected(object sender, EventArgs e)
    {
        _viewModel.change("ko");
        Close();
    }

    private void OnEnglishSelected(object sender, EventArgs e)
    {
        _viewModel.change("en");
        Close();
    }
}
