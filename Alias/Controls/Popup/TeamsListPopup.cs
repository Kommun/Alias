using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls.Primitives;
using Alias.Model;

namespace Alias.Controls
{
    public sealed class TeamsListPopup : ContentControl
    {
        private object _selectedItem;

        /// <summary>
        /// Выбранный элемент списка
        /// </summary>
        public object SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                Close();
            }
        }

        #region HintProperty 

        /// <summary>
        /// Подсказка
        /// </summary>
        public string Hint
        {
            get { return (string)GetValue(HintProperty); }
            set { SetValue(HintProperty, value); }
        }

        public static readonly DependencyProperty HintProperty =
            DependencyProperty.Register("Hint", typeof(string), typeof(TeamsListPopup), null);
        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        public TeamsListPopup()
        {
            this.DefaultStyleKey = typeof(TeamsListPopup);
            Height = Window.Current.Bounds.Height;
            Width = Window.Current.Bounds.Width;
        }

        /// <summary>
        /// Закрыть 
        /// </summary>
        private void Close()
        {
            try
            {
                (this.Parent as Popup).IsOpen = false;
            }
            catch { }
        }
    }
}
