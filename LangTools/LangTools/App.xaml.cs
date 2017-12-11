using Models;
using Models.Interfaces;
using Prism.Logging;
using System;
using System.IO;
using System.Windows;
using ViewModels;
using ViewModels.Interfaces;

namespace LangTools
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ILoggerFacade Logger { get; private set; }
        /// <summary>
        /// Prepares environmental settings for app and starts.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            // Get app name from config file
            string appName = Tools.Settings.Read("appName");
            if (string.IsNullOrWhiteSpace(appName))
            {
                MessageBox.Show("Error reading app settings.\nLangTools can't start.");
                return;
            }

            // Get app directory
            string appDir;
            try
            {
                appDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            }
            catch (PlatformNotSupportedException)
            {
                MessageBox.Show("Platform is not supported.\nLangTools can't start.");
                return;
            }

            // Create app directory path
            appDir = Path.Combine(appDir, appName);
            Logger = new SimpleLogger(appDir);
            // Save application directory path for later
            Current.Properties["appDir"] = appDir;
            // Create directory if not exist
            try
            {
                Directory.CreateDirectory(appDir);
            }
            catch (Exception err)
            {
                MessageBox.Show(string.Format("LangTools can't start.\n{0}", err.Message));
                return;
            }
            // TODO
            //// Ensure we have data storage.
            //if (!ModelBoot.IsReadyToStart(appDir))
            //{
            //    NB  return Storage.CreateFile(appDir);
            //    MessageBox.Show(string.Format("Can't start the app."));
            //    return;
            //}

            Logger.Log("----The new session has started.----", Category.Debug, Priority.Medium);
            // Start the main window
            MainWindow = new MainWindow
            {
                Title = appName
            };
            // Inject dependencies
            IUIMainWindowService service = new MainWindowService(MainWindow);
            IDataProvider dataProvider = ModelFactory.Model;
            MainWindow.DataContext = new MainViewModel(service, dataProvider, Logger);
            MainWindow.Show();
        }

        /// <summary>
        /// General exception cather. Logs exceptions.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.Log(e.Exception.ToString(), Category.Exception, Priority.High);
        }
    }
}
