using blueapp.Views.Splash;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace blueapp.ViewModels
{
    public class LanguageViewModel : BaseViewModel
    {
        private ResourceManager _resourceManager;

        public LanguageViewModel()
        {
            _resourceManager = new ResourceManager("blueapp.Resources.Localization.AppResources", typeof(LanguageViewModel).Assembly);
            CultureInfo.CurrentUICulture = new CultureInfo(Preferences.Get("Language", "ko")); // 기본 언어 설정
        }

        public string this[string key]
        {
            get
            {
                var value = _resourceManager.GetString(key, CultureInfo.CurrentUICulture);
                return value ?? string.Empty; // null 참조 경고를 해결하기 위해 기본값 반환
            }
        }

        public void SetLanguage(int selectedIndex)
        {
            Preferences.Set("LanguageCode", selectedIndex);
            switch (selectedIndex)
            {
                case 0:
                    Change("ko");
                    break;
                case 1:
                    Change("en");
                    break;
            }
        }

        public void Change(string code)
        {
            Preferences.Set("Language", code);
            CultureInfo.CurrentUICulture = new CultureInfo(code);
            OnPropertyChanged(string.Empty); // 모든 바인딩된 속성 갱신
        }

        // private void ChangeAppLanguage(string languageCode)
        // {
        //     CultureInfo.CurrentCulture = new CultureInfo(languageCode);
        //     CultureInfo.CurrentUICulture = new CultureInfo(languageCode);
        // 
        //     if (Application.Current != null)
        //     {
        //         Application.Current.MainPage = new AppShell();
        //         Task.Run(async () =>
        //         {
        //             await Application.Current.MainPage.Dispatcher.DispatchAsync(async () =>
        //             {
        //                 //SettingsPage로 이동
        //                 await Shell.Current.GoToAsync("//SettingsPage");
        //             });
        //         });
        //     }
        // }
    }
}
