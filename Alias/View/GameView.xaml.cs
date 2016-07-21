using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
                await _vm.Next(velocity.Linear.Y < 0);
        }

        private async void UpArrow_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await _vm.Next(true);
        }

        private async void DownArrow_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await _vm.Next(false);
        }
    }
}
