using CommunityToolkit.Maui.Views;

namespace blueapp.Views.Settings;

public partial class InfoPopup : Popup
{
	internal InfoPopup()
	{
		InitializeComponent();
    }

    // �˾��� ���� �� ȣ��Ǵ� �޼���
    private void ClosePopup(object sender, EventArgs e)
    {
        Close();
    }
}