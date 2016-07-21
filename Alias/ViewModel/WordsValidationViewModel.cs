using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alias.Model;
using Alias.Utils;

namespace Alias.ViewModel
{
    public class WordsValidationViewModel
    {
        private bool _saved;

        /// <summary>
        /// Далее
        /// </summary>
        public RelayCommand AcceptCommand { get; set; }

        /// <summary>
        /// Информация об игре
        /// </summary>
        public GameStat GameStat { get; set; } = GameStat.Instance;



        /// <summary>
        /// Конструктор
        /// </summary>
        public WordsValidationViewModel()
        {
            AcceptCommand = new RelayCommand(Accept);
        }

        /// <summary>
        /// Далее
        /// </summary>
        /// <param name="parameter"></param>
        private async void Accept(object parameter = null)
        {
            await Save();
            App.NavigationService.GoBack();
        }

        /// <summary>
        /// Сохранить
        /// </summary>
        public async Task Save()
        {
            if (_saved)
                return;

            await GameStat.CurrentTeam.SaveScore();

            _saved = true;
        }
    }
}
