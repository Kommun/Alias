using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Alias.Utils;
using Alias.Model;

namespace Alias.ViewModel
{
    public class MainMenuViewModel : PropertyChangedBase
    {
        /// <summary>
        /// Иконка кнопки вкл/выкл звука
        /// </summary>
        public string SoundIcon
        {
            get { return App.Settings.IsSoundEnabled ? "\uE15D" : "\uE198"; }
        }

        /// <summary>
        /// Новая игра
        /// </summary>
        public RelayCommand NewGameCommand { get; set; }

        /// <summary>
        /// Продолжить
        /// </summary>
        public RelayCommand ContinueCommand { get; set; } = new RelayCommand((e) => App.NavigationService.Navigate(typeof(View.TotalScoreView)));

        /// <summary>
        /// Правила
        /// </summary>
        public RelayCommand RulesCommand { get; set; }

        /// <summary>
        /// Магазин
        /// </summary>
        public RelayCommand ShopCommand { get; set; } = new RelayCommand((e) => App.NavigationService.Navigate(typeof(View.ShopView)));

        /// <summary>
        /// Включить/выключить звук
        /// </summary>
        public RelayCommand SoundCommand { get; set; }

        /// <summary>
        /// Информация об игре
        /// </summary>
        public GameStat GameStat { get; set; } = GameStat.Instance;

        /// <summary>
        /// Конструктор
        /// </summary>
        public MainMenuViewModel()
        {
            NewGameCommand = new RelayCommand(NewGame);
            RulesCommand = new RelayCommand(Rules);
            SoundCommand = new RelayCommand(Sound);
        }

        /// <summary>
        /// Новая игра
        /// </summary>
        /// <param name="parameter"></param>
        private async void NewGame(object parameter = null)
        {
            if (GameStat.Started)
            {
                var success = await PopupManager.ShowMessageDialog("Вы действительно хотите начать новую игру?", false);
                if (!success)
                    return;
            }
            GameStat.Reset();
            App.NavigationService.Navigate(typeof(View.TeamsListView));
        }

        /// <summary>
        /// Правила
        /// </summary>
        /// <param name="parameter"></param>
        private async void Rules(object parameter = null)
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Resources/rules.txt"));
            PopupManager.ShowNotificationPopup(await FileIO.ReadTextAsync(file));
        }

        /// <summary>
        /// Включить/выключить звук
        /// </summary>
        /// <param name="parameter"></param>
        private void Sound(object parameter = null)
        {
            App.Settings.IsSoundEnabled = !App.Settings.IsSoundEnabled;
            OnPropertyChanged("SoundIcon");
        }
    }
}
