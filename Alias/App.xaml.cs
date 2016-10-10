using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Popups;
using Windows.ApplicationModel.Store;
using Alias.Utils;
using Alias.Model;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Alias
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
        public static NavigationService NavigationService { get; set; }
        public static AppSettings Settings { get; } = new AppSettings();

        private TransitionCollection transitions;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            // Если открыто уведомление - закрываем
            if (PopupManager.CurrentPopup != null)
            {
                e.Handled = true;
                PopupManager.CloseCurrentPopup();
            }
            // Пытаемся перейти назад
            else if (NavigationService.GoBack())
                e.Handled = true;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
            await GameStat.Restore();
            // Hide the status bar
            await StatusBar.GetForCurrentView().HideAsync();

            if (Settings.DBversion != DataBaseHelper.dbVersion)
            {
                await CopyDatabase();
                Settings.DBversion = DataBaseHelper.dbVersion;
            }

            Settings.Runs++;
            if (Settings.NotRated && Settings.Runs == 4)
            {
                MessageDialog msgbox = new MessageDialog("Вы пользуетесь приложением уже достаточно долго. Не могли бы Вы оставить о нем отзыв?");

                msgbox.Commands.Add(new UICommand("Да", async c =>
                {
                    await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" + CurrentApp.AppId));
                    Settings.NotRated = false;
                }));
                msgbox.Commands.Add(new UICommand("Нет"));

                await msgbox.ShowAsync();
                Settings.Runs = 0;
            }

            // Заполняем список доступных наборов слов
            foreach (var l in CurrentApp.LicenseInformation.ProductLicenses.Where(l => l.Value.IsActive))
                switch (l.Key)
                {
                    case "BigPack":
                        Settings.AvailablePacks.Add(1);
                        return;
                }

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                // TODO: change this value to a cache size that is appropriate for your application
                rootFrame.CacheSize = 1;

                // Set the default language
                rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // Removes the turnstile navigation for startup.
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;

                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(View.MainView), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        /// <summary>
        /// Скопировать базу данных в локальное хранилище
        /// </summary>
        /// <returns></returns>
        private async Task CopyDatabase()
        {
            try
            {
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///Resources/alias.db"));
                StorageFolder folder = ApplicationData.Current.LocalFolder;
                await file.CopyAsync(folder, "alias.db", NameCollisionOption.ReplaceExisting);
            }
            catch { }
        }
    }
}