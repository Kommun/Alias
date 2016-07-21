using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alias.Utils;

namespace Alias.Model
{
    public class Team : PropertyChangedBase
    {
        private string _name;
        private bool _playedCurrentRound;
        public int _currentScore;

        /// <summary>
        /// Название
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Список слов
        /// </summary>
        public List<GameWord> Words { get; set; } = new List<GameWord>();

        /// <summary>
        /// Количество угаданных слов
        /// </summary>
        public int GuessedCount { get { return Words.Count(w => w.IsGuessed ?? false); } }

        /// <summary>
        /// Количество неугаданных слов
        /// </summary>
        public int NotGuessedCount { get { return -Words.Count(w => !w.IsGuessed ?? false); } }

        /// <summary>
        /// Текущий раунд сыгран
        /// </summary>
        public bool PlayedCurrentRound
        {
            get { return _playedCurrentRound; }
        }

        /// <summary>
        /// Суммарный счет
        /// </summary>
        public int TotalScore { get; set; }

        /// <summary>
        /// Счет в текущем раунде
        /// </summary>
        public int CurrentScore
        {
            get { return _currentScore; }
            set
            {
                TotalScore += value - _currentScore;
                _currentScore = value;
            }
        }

        /// <summary>
        /// Сохранить счет
        /// </summary>
        /// <returns></returns>
        public async Task SaveScore()
        {
            _playedCurrentRound = true;
            CurrentScore += GuessedCount + NotGuessedCount;
            Words.Clear();
            await GameStat.Save();
        }

        /// <summary>
        /// Сбросить счет
        /// </summary>
        public void ResetScore()
        {
            _playedCurrentRound = false;
            _currentScore = 0;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
