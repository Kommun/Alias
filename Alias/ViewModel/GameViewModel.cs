using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Alias.Model;
using Alias.Utils;

namespace Alias.ViewModel
{
    public class GameViewModel : PropertyChangedBase
    {
        public event EventHandler TimeRunsOut;

        private TimeSpan _timeLast;
        private MediaElement _player;
        private DispatcherTimer _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };

        /// <summary>
        /// Информация об игре
        /// </summary>
        public GameStat GameStat { get; set; } = GameStat.Instance;

        /// <summary>
        /// Оставшееся время
        /// </summary>
        public TimeSpan TimeLast
        {
            get { return _timeLast; }
            set
            {
                _timeLast = value;
                OnPropertiesChanged("TimeLast");
            }
        }

        /// <summary>
        /// Текущее слово
        /// </summary>
        public GameWord CurrentWord
        {
            get { return GameStat.CurrentTeam.Words.Last(); }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public GameViewModel(MediaElement player)
        {
            _player = player;
            AddWord();
            TimeLast = TimeSpan.FromSeconds(GameStat.Duration);
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        private void _timer_Tick(object sender, object e)
        {
            TimeLast -= _timer.Interval;

            if (TimeLast.TotalSeconds == 9 && TimeRunsOut != null)
                TimeRunsOut(this, EventArgs.Empty);

            if (TimeLast.TotalSeconds <= 0)
            {
                _timer.Stop();
                Play("ms-appx:///Resources/timeout.mp3");
            }
        }

        /// <summary>
        /// Перейти к следующему слову
        /// </summary>
        /// <param name="isGuessed">Угадано ли текущее слово</param>
        public async Task Next(bool isGuessed)
        {
            Play(isGuessed ? "ms-appx:///Resources/guessed.wav" : "ms-appx:///Resources/notguessed.wav", 1);

            if (_timeLast.TotalSeconds == 0)
            {
                if (GameStat.IsLastCommon)
                {
                    if (isGuessed)
                    {
                        var team = await PopupManager.ShowTeamsListDialog(GameStat.Teams, "Какая команда угадала последнее слово?");
                        if (team != null)
                            team.CurrentScore++;
                    }
                    GameStat.CurrentTeam.Words.Remove(CurrentWord);
                }
                else
                    CurrentWord.IsGuessed = isGuessed;

                await Task.Delay(500);
                // Если есть слова - переходим к корректировке
                if (GameStat.CurrentTeam.Words.Count > 0)
                    App.NavigationService.Navigate(typeof(View.WordsValidationView), GameStat.CurrentTeam.Words);
                // Если слов нет - сразу переходим к общему счету
                else
                {
                    await GameStat.CurrentTeam.SaveScore();
                    App.NavigationService.GoBack();
                }
            }
            else
            {
                CurrentWord.IsGuessed = isGuessed;
                AddWord();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Добавить слово
        /// </summary>
        private void AddWord()
        {
            var query = string.Format("Select * from Word join Theme on Word.ThemeId=Theme.ThemeId"
                + " where Word.ThemeId in({0}) and Word.WordId not in({1}) order by random() limit 1", GameStat.ThemesId, GameStat.WordsId);
            var word = DataBaseHelper.Connection.Query<GameWord>(query).FirstOrDefault();
            if (word == null)
            {
                GameStat.WordsId = null;
                AddWord();
                return;
            }

            GameStat.WordsId += GameStat.WordsId == null ? word.WordId.ToString() : $",{word.WordId}";
            GameStat.CurrentTeam.Words.Add(word);
        }

        /// <summary>
        /// Воспроизвести звук
        /// </summary>
        /// <param name="soundPath">Путь к файлу</param>
        /// <param name="volume">Громкость</param>
        public void Play(string soundPath, double volume = 1)
        {
            if (AppSettings.Instance.IsSoundEnabled)
                try
                {
                    _player.Volume = volume;
                    _player.Source = new Uri(soundPath);
                }
                catch { }
        }

        /// <summary>
        /// Остановить таймер
        /// </summary>
        public void Stop()
        {
            _timer.Stop();
        }
    }
}
