using blueapp.ViewModels;
using blueapp.Views.Splash;
using System.ComponentModel;

namespace blueapp
{
    public partial class App : Application, INotifyPropertyChanged
    {
        public static new App? Current => Application.Current as App;
        public static LanguageViewModel? LanguageViewModel { get; private set; }

        public App()
        {
            InitializeComponent();
            LoadPreferences();
            LanguageViewModel = new LanguageViewModel();
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
            
            var backgroundColor = isDarkMode ? Color.FromArgb("#286692") : Color.FromArgb("#cbe9ff");
            if(Resources.TryGetValue("DefaultPageBackgroundColor", out var resources))
            {
                Resources["DefaultPageBackgroundColor"] = backgroundColor;
            }
        }
    }
}
