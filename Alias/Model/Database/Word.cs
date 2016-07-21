using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Alias.Model.Database
{
    public class Word
    {
        /// <summary>
        /// ID
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int WordId { get; set; }

        /// <summary>
        /// Слово
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// ID темы
        /// </summary>
        public string ThemeId { get; set; }
    }
}
