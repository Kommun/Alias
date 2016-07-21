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
using Alias.Utils;

namespace Alias.View
{
    public sealed partial class WordsValidationView : Page
    {
        private WordsValidationViewModel _vm;

        public WordsValidationView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _vm = new WordsValidationViewModel();
            DataContext = _vm;
            App.NavigationService.DeleteLastFromBackStack();
        }

        protected async override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            await _vm.Save();
        }
    }
}
