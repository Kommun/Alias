using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Animation;
using Alias.Model;
using Alias.ViewModel;

namespace Alias.View
{
    public sealed partial class GameView : Page
    {
        private GameViewModel _vm;

        /// <summary>
        /// Конструктор
        /// </summary>
        public GameView()
        {
            this.InitializeComponent();
            _vm = new GameViewModel(player);
            _vm.TimeRunsOut += (s, e) => (Resources["timeAnimation"] as Storyboard).Begin();
            DataContext = _vm;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _vm.Stop();
        }

        /// <summary>
        /// Начало манипуляции с карточкой
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            e.Complete();
        }

        /// <summary>
        /// Конец манипуляции с карточкой
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Grid_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            var velocity = e.Velocities;
            if (Math.Abs(velocity.Linear.Y) > Math.Abs(velocity.Linear.X))
                await GoToNextWord(velocity.Linear.Y < 0);
        }

        /// <summary>
        /// Обработчик нажатия на верхнюю стрелку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void UpArrow_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await GoToNextWord(true);
        }

        /// <summary>
        /// Обработчик нажатия на нижнюю стрелку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DownArrow_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await GoToNextWord(false);
        }

        /// <summary>
        /// Перейти к следующему слову
        /// </summary>
        /// <param name="isGuessed">Угадано ли текущее слово</param>
        /// <returns></returns>
        private async Task GoToNextWord(bool isGuessed)
        {
            tbWord.FontSize = 40;
            await _vm.Next(isGuessed);
        }

        /// <summary>
        /// Обработчик изменения размера текстового поля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbWord_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (tbWord.ActualWidth > grdCard.ActualWidth)
                tbWord.FontSize--;
        }
    }
}
