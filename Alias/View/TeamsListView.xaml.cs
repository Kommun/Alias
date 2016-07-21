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
using Alias.Model;

namespace Alias.View
{
    public sealed partial class TeamsListView : Page
    {
        private TeamsListViewModel _vm;
        private FlyoutBase _contextMenu;

        public TeamsListView()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode != NavigationMode.Back)
            {
                _vm = new TeamsListViewModel();
                DataContext = _vm;
            }
        }

        /// <summary>
        /// Удалить команду
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            _vm.DeleteTeam((e.OriginalSource as FrameworkElement).DataContext as Team);
        }

        /// <summary>
        /// Изменить название команды
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var team = (e.OriginalSource as FrameworkElement).DataContext as Team;
            _contextMenu.Hide();
            await System.Threading.Tasks.Task.Delay(100);
            _vm.EditTeam(team);
        }

        /// <summary>
        /// Нажатие на элемент списка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            _contextMenu = FlyoutBase.GetAttachedFlyout(senderElement);
            _contextMenu.ShowAt(senderElement);
        }
    }
}
