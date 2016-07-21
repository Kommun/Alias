using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alias.Model;
using Alias.Model.Database;
using Alias.Utils;

namespace Alias.Model
{
    public class GameStat
    {
        private static GameStat _instance;
        private Team _currentTeam;

        public static GameStat Instance
        {
            get { return _instance ?? (_instance = new GameStat()); }
            private set { _instance = value; }
        }

        private static SerializationManager _serializationManager = new SerializationManager("savedata");

        protected GameStat() { }

        #region Properies

        /// <summary>
        /// Началась ли игра
        /// </summary>
        public bool Started { get; set; }

        /// <summary>
        /// Список команд
        /// </summary>
        public ObservableCollection<Team> Teams { get; set; } = new ObservableCollection<Team>();

        /// <summary>
        /// Текущая команда
        /// </summary>
        public Team CurrentTeam
        {
            get
            {
                if (_currentTeam == null || _currentTeam.PlayedCurrentRound)
                    _currentTeam = Teams.FirstOrDefault(t => !t.PlayedCurrentRound);

                return _currentTeam;
            }
        }

        /// <summary>
        /// Раунд
        /// </summary>
        public int Round { get; set; } = 1;

        /// <summary>
        /// Наборы слов
        /// </summary>
        public string ThemesId { get; set; }

        /// <summary>
        /// Список слов текущей игры
        /// </summary>
        public string WordsId { get; set; }

        /// <summary>
        /// Длительность раунда
        /// </summary>
        public int Duration { get; set; } = 60;

        /// <summary>
        /// Количество очков, необходимое для победы
        /// </summary>
        public int Goal { get; set; } = 100;

        /// <summary>
        /// Последнее слово общее
        /// </summary>
        public bool IsLastCommon { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Новая игра
        /// </summary>
        public static async void Reset()
        {
            Instance = null;
            await _serializationManager.Delete();
        }

        /// <summary>
        /// Сохранить данные
        /// </summary>
        /// <returns></returns>
        public static async Task Save()
        {
            await _serializationManager.Save(Instance);
        }

        /// <summary>
        /// Восстановить сохраненные данные
        /// </summary>
        /// <returns></returns>
        public static async Task Restore()
        {
            var data = await _serializationManager.Restore<GameStat>();
            if (data != null)
                Instance = data;
        }

        #endregion
    }
}
