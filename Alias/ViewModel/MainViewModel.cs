using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alias.Utils;

namespace Alias.ViewModel
{
    public class MainViewModel
    {
        /// <summary>
        /// Настройки
        /// </summary>
        public AppSettings Settings
        {
            get { return App.Settings; }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public MainViewModel()
        {
            App.NavigationService.Navigate(typeof(View.MainMenuView));
        }
    }
}
