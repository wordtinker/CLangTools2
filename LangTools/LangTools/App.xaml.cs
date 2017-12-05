using Prism.Logging;
using System.Windows;
using ViewModels.Interfaces;
using ViewModels;
using Models.Interfaces;
using Models;
using System;
// TODO fix usings order across all projects

namespace LangTools
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // TODO non empty logger
        public static ILoggerFacade Logger { get; private set; } = new EmptyLogger();
        /// <summary>
        /// Prepares environmental settings for app and starts.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            // Get app name from config file
            string appName = Tools.ReadSetting("appName");
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

            // TODO STUB
            // TODO Properties? another way?
            //appDir = IOTools.CombinePath(appDir, appName);
            //// Save application directory path for later
            //Current.Properties["appDir"] = appDir;

            //// Create directory if not exist
            //if (!IOTools.CreateDirectory(appDir))
            //{
            //    MessageBox.Show(string.Format("Something bad happened in {0} directory.\nLangTools can't start.", appDir));
            //    return;
            //}

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
            IDataProvider dataProvider = new StubModel();
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
