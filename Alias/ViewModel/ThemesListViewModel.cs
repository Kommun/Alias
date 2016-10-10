using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alias.View;
using Alias.Model;
using Alias.Model.Database;
using Alias.Utils;

namespace Alias.ViewModel
{
    public class ThemesListViewModel
    {
        /// <summary>
        /// Купить наборы карт
        /// </summary>
        public RelayCommand ShopCommand { get; set; } = new RelayCommand(e => App.NavigationService.Navigate(typeof(View.ShopView)));

        /// <summary>
        /// Список тем игры
        /// </summary>
        public List<Theme> Themes { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ThemesListViewModel()
        {
            var query = "Select * from Theme";
            if (!App.Settings.IsFullVersion)
                query += $" where PackId in ({string.Join(",", App.Settings.AvailablePacks)})";

            Themes = DataBaseHelper.Connection.Query<Theme>(query);
        }

        /// <summary>
        /// Начать игру
        /// </summary>
        /// <param name="themes"></param>
        public void StartGame(IEnumerable<object> themes)
        {
            if (themes.Count() > 0)
            {
                GameStat.Instance.ThemesId = string.Join(",", themes.Select(t => (t as Theme).ThemeId));
                GameStat.Instance.Started = true;
                App.NavigationService.Navigate(typeof(TotalScoreView));
            }
            else
                PopupManager.ShowNotificationPopup("Выберите хотя бы одну тему");
        }
    }
}
