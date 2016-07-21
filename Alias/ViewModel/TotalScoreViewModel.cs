using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alias.Model;
using Alias.Utils;

namespace Alias.ViewModel
{
    public class TotalScoreViewModel
    {
        /// <summary>
        /// Продолжить
        /// </summary>
        public RelayCommand ContinueCommand { get; set; }

        /// <summary>
        /// Информация о текущей игре
        /// </summary>
        public GameStat GameStat { get; set; } = GameStat.Instance;

        /// <summary>
        /// Номер текущей игры
        /// </summary>
        public int GameNumber
        {
            get { return GameStat.Teams.Count(t => t.PlayedCurrentRound) + 1; }
        }

        /// <summary>
        /// Отсортированные команды
        /// </summary>
        public IEnumerable<object> OrderedTeams
        {
            get
            {
                int index = 0;
                return GameStat.Teams.OrderByDescending(t => t.TotalScore).Select(t => new
                {
                    Team = t,
                    ScoreColor = t.CurrentScore > 0 ? "Teal" : t.CurrentScore == 0 ? "Gray" : "Firebrick",
                    Index = ++index
                });
            }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public TotalScoreViewModel()
        {
            ContinueCommand = new RelayCommand(Continue);

            if (GameStat.CurrentTeam == null)
            {
                // Обнуляем текущий счет
                foreach (var t in GameStat.Teams)
                    t.ResetScore();

                GameStat.Round++;
            }

            var winner = GameStat.Teams.FirstOrDefault(t => t.TotalScore >= GameStat.Goal);
            if (winner != null)
            {
                PopupManager.ShowNotificationPopup($"Победу одержала команда \"{winner.Name}\". Поздравляем!");
                GameStat.Reset();
            }
        }

        /// <summary>
        /// Продолжить
        /// </summary>
        private void Continue(object parameter = null)
        {
            if (GameStat.Teams.FirstOrDefault(t => t.TotalScore >= GameStat.Goal) == null)
                App.NavigationService.Navigate(typeof(View.GameView));
            else
                App.NavigationService.GoBack();
        }
    }
}
