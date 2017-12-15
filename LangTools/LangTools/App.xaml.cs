using Models;
using Models.Interfaces;
using Prism.Logging;
using Shared.Tools;
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
                appDir = Path.Combine(appDir, appName);
                Directory.CreateDirectory(appDir);
            }
            catch (Exception err)
            {
                MessageBox.Show(string.Format("LangTools can't start.\n{0}", err.Message));
                return;
            }

            // Configure and start model
            IO.CombinePath(out string stylePath, Directory.GetCurrentDirectory(), "plugins");
            Config.StyleDirectoryPath = stylePath;
            Config.CommonDictionaryName = Tools.Settings.Read("commonDic");
            Config.WorkingDirectory = appDir;
            IDataProvider dataProvider = ModelFactory.Model;
            // Create logger
            Logger = new SimpleLogger(appDir);
            // Start main window
            MainWindow = new MainWindow
            {
                Title = appName
            };
            // Inject dependencies and properties
            IUIMainWindowService service = new MainWindowService(MainWindow);
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
