namespace blueapp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            BindingContext = App.LanguageViewModel;
        }
    }
}
