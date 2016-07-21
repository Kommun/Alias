using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alias.Model;
using Alias.Utils;

namespace Alias.ViewModel
{
    public class GameSettingsViewModel
    {
        /// <summary>
        /// Дальше
        /// </summary>
        public RelayCommand AcceptCommand { get; set; } = new RelayCommand((e) => App.NavigationService.Navigate(typeof(View.ThemesListView)));

        /// <summary>
        /// Информация об игре
        /// </summary>
        public GameStat GameStat { get; set; } = GameStat.Instance;
    }
}
