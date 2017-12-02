using System.Windows;
using ViewModels.Interfaces;

namespace LangTools
{
    class MainWindowService : IUIMainWindowService
    {
        //private MainWindow mainWindow;

        // TODO mainwindow or app or none?
        public MainWindowService(MainWindow mainWindow)
        {
            //this.mainWindow = mainWindow;
        }

        public void ShowHelp()
        {
            MessageBox.Show("!!!");
            // TODO STUB 3 assemblies
            //string.Format("{0}: {1}", windowService.AppName, CoreAssembly.Version);
        }

        public void Shutdown()
        {
            App.Current.Shutdown();
        }
        // TODO
    }
}
