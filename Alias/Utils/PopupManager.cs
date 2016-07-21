using Windows.UI;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;
using System.Threading.Tasks;
using Alias.Controls;
using Alias.Model;

namespace Alias.Utils
{
    public static class PopupManager
    {
        /// <summary>
        /// Текущее уведомление
        /// </summary>
        public static Popup CurrentPopup { get; set; }

        /// <summary>
        /// Создает уведомление
        /// </summary>
        /// <returns></returns>
        public static void ShowNotificationPopup(string message)
        {
            var popup = new Popup();
            popup.ChildTransitions = new TransitionCollection { new ContentThemeTransition() };
            popup.Child = new NotificationMessage { Content = message };
            popup.Closed += (a, e) =>
            {
                App.NavigationService.SetAppBarVisibility(true);
                CurrentPopup = null;
            };
            App.NavigationService.SetAppBarVisibility(false);
            CurrentPopup = popup;
            popup.IsOpen = true;
        }

        /// <summary>
        /// Показать всплывающий список
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public async static Task<T> ShowTeamsListDialog<T>(IEnumerable<T> items, string hint)
        {
            var dialog = new TeamsListPopup()
            {
                Content = items,
                Hint = hint
            };
            await ShowDialog(dialog);
            return (T)dialog.SelectedItem;
        }

        /// <summary>
        /// Показать диалог с выбором ответа
        /// </summary>
        /// <param name="message"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public async static Task<bool> ShowMessageDialog(string message, bool defaultValue)
        {
            var dialog = new MessageDialogPopup
            {
                Content = message,
                Result = defaultValue
            };
            await ShowDialog(dialog);
            return dialog.Result;
        }

        /// <summary>
        /// Показать диалог ввода
        /// </summary>
        /// <param name="hint"></param>
        /// <returns></returns>
        public async static Task<string> ShowInputDialog(string hint)
        {
            var textBox = new TextBox();
            var dialog = new InputDialog
            {
                Content = textBox,
                Hint = hint
            };
            await ShowDialog(dialog);
            return textBox.Text;
        }

        /// <summary>
        /// Показать диалог
        /// </summary>
        /// <param name="dialog"></param>
        /// <returns></returns>
        public async static Task ShowDialog(Control dialog)
        {
            bool closed = false; ;
            var popup = new Popup();
            popup.ChildTransitions = new TransitionCollection { new ContentThemeTransition() };

            popup.Child = dialog;
            popup.Closed += (a, e) =>
            {
                App.NavigationService.SetAppBarVisibility(true);
                CurrentPopup = null;
                closed = true;
            };
            App.NavigationService.SetAppBarVisibility(false);
            CurrentPopup = popup;
            popup.IsOpen = true;
            await Task.Factory.StartNew(() => { while (!closed) { } });
        }

        /// <summary>
        /// Закрыть текущее уведомление
        /// </summary>
        public static void CloseCurrentPopup()
        {
            if (CurrentPopup != null)
            {
                CurrentPopup.IsOpen = false;
                CurrentPopup = null;
            }
        }
    }
}
