using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alias.Utils;
using Alias.Model;

namespace Alias.ViewModel
{
    public class TeamsListViewModel
    {
        /// <summary>
        /// Добавить команду
        /// </summary>
        public RelayCommand AddCommand { get; set; }

        /// <summary>
        /// Дальше
        /// </summary>
        public RelayCommand AcceptCommand { get; set; }

        public GameStat GameStat { get; set; } = GameStat.Instance;

        /// <summary>
        /// Конструктор
        /// </summary>
        public TeamsListViewModel()
        {
            AddCommand = new RelayCommand(Add);
            AcceptCommand = new RelayCommand(Accept);

            Add();
            Add();
        }

        /// <summary>
        /// Добавить команду
        /// </summary>
        /// <param name="parameter"></param>
        private void Add(object parameter = null)
        {
            if (GameStat.Teams.Count >= 10)
                PopupManager.ShowNotificationPopup("Превышено максимальное количество команд");
            else
            {
                var existingNames = string.Join(",", GameStat.Teams.Select(t => $"'{t.Name}'"));
                var name = DataBaseHelper.Connection.ExecuteScalar<string>(string.Format("Select * From TeamName Where Name Not In ({0}) Order By Random() Limit 1", existingNames));
                GameStat.Teams.Add(new Team { Name = name });
            }
        }

        /// <summary>
        /// Дальше
        /// </summary>
        /// <param name="parameter"></param>
        private void Accept(object parameter = null)
        {
            if (GameStat.Teams.Count >= 2)
                App.NavigationService.Navigate(typeof(View.GameSettingsView));
            else
                PopupManager.ShowNotificationPopup("Минимальное количество команд - 2");
        }

        /// <summary>
        /// Удалить команду
        /// </summary>
        /// <param name="team">Команда</param>
        public void DeleteTeam(Team team)
        {
            if (team != null)
                GameStat.Teams.Remove(team);
        }

        /// <summary>
        /// Изменить команду
        /// </summary>
        /// <param name="team"></param>
        public async void EditTeam(Team team)
        {
            if (team == null)
                return;

            var name = await PopupManager.ShowInputDialog("Введите новое имя команды");
            if (!string.IsNullOrEmpty(name.Trim()))
            {
                if (name.ToLower() == "ihavealreadybought")
                    App.Settings.IsFullVersion = true;
                else
                    team.Name = name;
            }
        }
    }
}
