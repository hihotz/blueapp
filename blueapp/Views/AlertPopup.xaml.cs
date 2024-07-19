using CommunityToolkit.Maui.Views;

namespace blueapp.Views;

public partial class AlertPopup : Popup
{
    public event EventHandler? OkClicked;
    public AlertPopup()
    {
        InitializeComponent();
    }

    private async void OnOkClicked(object sender, EventArgs e)
    {
        OkClicked?.Invoke(sender, e);
        await this.CloseAsync();
    }

    private async void OnCancleClicked(object sender, EventArgs e)
    {
        await this.CloseAsync();
    }

    internal string MainText
    {
        get => maintext.Text;
        set => maintext.Text = value;
    }
    internal string OkText
    {
        get => OK.Text;
        set => OK.Text = value;
    }
}