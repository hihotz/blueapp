using blueapp.Views.Splash;
using System.ComponentModel;

namespace blueapp
{
    public partial class App : Application, INotifyPropertyChanged
    {
        public static new App? Current => Application.Current as App;
        public App()
        {
            InitializeComponent();
            LoadPreferences();
            MainPage = new SplashPage();
        }

        public void LoadPreferences()
        {
            var isDarkMode = Preferences.Get("IsDarkMode", false);
            ApplyTheme(isDarkMode);
            // 앱 언어 선택 추가는 이곳에
            // var language = Preferences.Get("Language", "ko");
        }

        public void ApplyTheme(bool isDarkMode)
        {
            var currentTheme = isDarkMode ? AppTheme.Dark : AppTheme.Light;
            UserAppTheme = currentTheme;
            
            var backgroundColor = isDarkMode ? Color.FromArgb("#121212") : Color.FromArgb("#fafafa");
            if(Resources.TryGetValue("DefaultPageBackgroundColor", out var resources))
            {
                Resources["DefaultPageBackgroundColor"] = backgroundColor;
            }
        }
    }
}
