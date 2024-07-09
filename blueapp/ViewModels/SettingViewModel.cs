using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blueapp.ViewModels
{
    public class SettingViewModel : BaseViewModel
    {
        private bool _isDarkMode;
        public SettingViewModel()
        {
            IsDarkMode = Preferences.Get("IsDarkMode", false);
        }

        public bool IsDarkMode
        {
            get => _isDarkMode;
            set
            {
                SetProperty(ref _isDarkMode, value);
                Preferences.Set("IsDarkMode", value);
                if (App.Current != null)
                {
                    App.Current.ApplyTheme(value);
                }
            }
        }
    }
}
