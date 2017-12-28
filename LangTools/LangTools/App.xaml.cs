using Models.Interfaces;
using Prism.Events;
using Prism.Logging;
using Shared.Tools;
using System;
using System.IO;
using System.Windows;
using Unity;
using Unity.Resolution;
using ViewModels;
using ViewModels.Interfaces;

namespace LangTools
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IUnityContainer Container { get; private set; }
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
            var modelFactory = new ModelFactory.ModelFactory(appDir, stylePath, Tools.Settings.Read("commonDic"));
            // Make unity container out of model container
            Container = modelFactory.Container.CreateChildContainer();
            // Register event aggregator
            Container.RegisterInstance<IEventAggregator>(new EventAggregator());
            // Register logging class
            Container.RegisterInstance<ILoggerFacade>(new SimpleLogger(appDir));
            // Start main window
            MainWindow = new MainWindow
            {
                Title = appName
            };
            // Inject dependencies and properties
            IUIMainWindowService service = new MainWindowService(MainWindow);
            MainWindow.DataContext = Container.Resolve<MainViewModel>(new ParameterOverride("windowService", service));
            MainWindow.Show();
        }

        /// <summary>
        /// General exception cather. Logs exceptions.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Container.Resolve<ILoggerFacade>()
                .Log(e.Exception.ToString(), Category.Exception, Priority.High);
        }
    }
}
