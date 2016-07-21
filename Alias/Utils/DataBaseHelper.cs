using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using SQLite;

namespace Alias
{
    public static class DataBaseHelper
    {
        public const int dbVersion = 2;

        private static string _dbPath = System.IO.Path.Combine(System.IO.Path.Combine(ApplicationData.Current.LocalFolder.Path, "alias.db"));
        private static SQLiteConnection _connection;

        /// <summary>
        /// Соединение
        /// </summary>
        public static SQLiteConnection Connection
        {
            get
            {
                return _connection ?? (_connection = new SQLiteConnection(_dbPath));
            }
        }
    }
}
