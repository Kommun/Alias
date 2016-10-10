using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.ComponentModel;
using Windows.ApplicationModel.Background;
using Windows.Storage.Pickers;

namespace Alias.Utils
{
    public class AppSettings : PropertyChangedBase
    {
        private ApplicationDataContainer settings;

        /// <summary>
        /// Конструктор
        /// </summary>
        public AppSettings()
        {
            settings = ApplicationData.Current.LocalSettings;
        }

        /// <summary>
        /// Добавить или обновить значение
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="value"></param>
        public void AddOrUpdateValue(string Key, object value)
        {
            if (settings.Values[Key] != value)
                settings.Values[Key] = value;
            OnPropertyChanged(Key);
        }

        /// <summary>
        /// Извлечить значение
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetValueOrDefault<T>(string Key, T defaultValue)
        {
            T value;

            if (settings.Values.ContainsKey(Key))
                value = (T)settings.Values[Key];
            else
                value = defaultValue;

            return value;
        }

        #region Launch 

        public int Runs
        {
            get { return GetValueOrDefault<int>("Runs", 0); }
            set { AddOrUpdateValue("Runs", value); }
        }

        public bool NotRated
        {
            get { return GetValueOrDefault<bool>("NotRated", true); }
            set { AddOrUpdateValue("NotRated", value); }
        }

        public bool IsFirstLaunch
        {
            get { return GetValueOrDefault<bool>("IsFirstLaunch", true); }
            set { AddOrUpdateValue("IsFirstLaunch", value); }
        }

        #endregion

        public List<int> AvailablePacks { get; set; } = new List<int>();

        public int DBversion
        {
            get { return GetValueOrDefault<int>("DBversion", 0); }
            set { AddOrUpdateValue("DBversion", value); }
        }

        public bool IsSoundEnabled
        {
            get { return GetValueOrDefault<bool>("IsSoundEnabled", true); }
            set { AddOrUpdateValue("IsSoundEnabled", value); }
        }

        public bool IsFullVersion
        {
            get { return GetValueOrDefault<bool>("IsFullVersion", false); }
            set { AddOrUpdateValue("IsFullVersion", value); }
        }
    }
}