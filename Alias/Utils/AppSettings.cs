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
        private static AppSettings _instance;
        public static AppSettings Instance
        {
            get { return _instance ?? (_instance = new AppSettings()); }
        }

        protected AppSettings()
        {
            settings = ApplicationData.Current.LocalSettings;
        }

        ApplicationDataContainer settings;

        public void AddOrUpdateValue(string Key, Object value)
        {
            if (settings.Values[Key] != value)
                settings.Values[Key] = value;
            OnPropertyChanged(Key);
        }

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