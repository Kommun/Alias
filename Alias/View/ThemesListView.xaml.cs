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
using Alias.ViewModel;

namespace Alias.View
{
    public sealed partial class ThemesListView : Page
    {
        private ThemesListViewModel _vm;

        public ThemesListView()
        {
            this.InitializeComponent();
            _vm = new ThemesListViewModel();
            DataContext = _vm;
        }

        /// <summary>
        /// Начать игру
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            _vm.StartGame(lvThemes.SelectedItems);
        }
    }
}
