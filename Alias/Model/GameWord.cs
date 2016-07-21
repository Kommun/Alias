using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alias.Utils;

namespace Alias.Model
{
    public class GameWord : PropertyChangedBase
    {
        private bool? _isGuessed;

        /// <summary>
        /// Угадано
        /// </summary>
        public RelayCommand PlusCommand { get; set; } = new RelayCommand(w => (w as GameWord).IsGuessed = true);

        /// <summary>
        /// Не угадано
        /// </summary>
        public RelayCommand MinusCommand { get; set; } = new RelayCommand(w => (w as GameWord).IsGuessed = false);

        /// <summary>
        /// Удалить
        /// </summary>
        public RelayCommand DeleteCommand { get; set; } = new RelayCommand(w => (w as GameWord).IsGuessed = null);

        /// <summary>
        /// ID слова
        /// </summary>
        public int WordId { get; set; }

        /// <summary>
        /// Тема
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Слово
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Угадано (да, нет, не засчитывать)
        /// </summary>
        public bool? IsGuessed
        {
            get { return _isGuessed; }
            set
            {
                _isGuessed = value;
                OnPropertyChanged("IsGuessed");
            }
        }
    }
}
