using blueapp.Views.Splash;

namespace blueapp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new SplashPage();
        }
    }
}
