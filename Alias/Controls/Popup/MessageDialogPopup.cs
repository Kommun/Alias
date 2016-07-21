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
    public sealed class MessageDialogPopup : ContentControl
    {
        #region ResultProperty 

        /// <summary>
        /// Результат диалога
        /// </summary>
        public bool Result
        {
            get { return (bool)GetValue(ResultProperty); }
            set { SetValue(ResultProperty, value); }
        }

        public static readonly DependencyProperty ResultProperty =
            DependencyProperty.Register("Result", typeof(bool), typeof(MessageDialogPopup), null);
        #endregion

        public MessageDialogPopup()
        {
            this.DefaultStyleKey = typeof(MessageDialogPopup);
            Height = Window.Current.Bounds.Height;
            Width = Window.Current.Bounds.Width;
        }

        protected override void OnApplyTemplate()
        {
            var btnYes = GetTemplateChild("btnYes") as CustomButton;
            btnYes.Click += BtnYes_Click;
            var btnNo = GetTemplateChild("btnNo") as CustomButton;
            btnNo.Click += BtnNo_Click;
        }

        private void BtnNo_Click(object sender, RoutedEventArgs e)
        {
            Result = false;
            Close();
        }

        private void BtnYes_Click(object sender, RoutedEventArgs e)
        {
            Result = true;
            Close();
        }

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
