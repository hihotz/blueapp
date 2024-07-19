using CommunityToolkit.Maui.Views;

namespace blueapp.Views.Settings;

public partial class InfoPopup : Popup
{
	internal InfoPopup()
	{
		InitializeComponent();
    }

    // ÆË¾÷ÀÌ ´ÝÈú ¶§ È£ÃâµÇ´Â ¸Þ¼­µå
    private void ClosePopup(object sender, EventArgs e)
    {
        Close();
    }
}