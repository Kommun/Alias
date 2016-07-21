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

namespace Alias.Controls
{
    public class InputDialog : ContentControl
    {
        #region ValueProperty 

        /// <summary>
        /// Значение
        /// </summary>
        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(string), typeof(InputDialog), null);
        #endregion

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
            DependencyProperty.Register("Hint", typeof(string), typeof(InputDialog), null);
        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        public InputDialog()
        {
            this.DefaultStyleKey = typeof(InputDialog);
            Height = Window.Current.Bounds.Height;
            Width = Window.Current.Bounds.Width;
            Loaded += InputDialog_Loaded;
        }

        /// <summary>
        /// Обработчик загрузки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InputDialog_Loaded(object sender, RoutedEventArgs e)
        {
            var tbValue = (GetTemplateChild("contentPresenter") as ContentPresenter).Content as TextBox;
            tbValue.Focus(FocusState.Programmatic);
        }

        protected override void OnApplyTemplate()
        {
            var btnClose = GetTemplateChild("btnClose") as CustomButton;
            btnClose.Click += btnClose_Click;
        }

        /// <summary>
        /// Закрыть подсказку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                (this.Parent as Popup).IsOpen = false;
            }
            catch { }
        }

    }
}
