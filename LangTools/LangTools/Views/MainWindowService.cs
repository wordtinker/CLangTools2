using System.Windows;
using ViewModels.Interfaces;
using ViewModels;
using Models;

namespace LangTools
{
    class MainWindowService : IUIMainWindowService
    {
        private Window mainWindow;

        public MainWindowService(Window mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        public void ManageLanguages()
        {
            // TODO more elegant ? model.getvalidator?
            LangWindowViewModel vm = new LangWindowViewModel(
                (MainViewModel)mainWindow.DataContext, this, new LingvaValidator());
            LangManager window = new LangManager
            {
                DataContext = vm,
                // Ensure the alt+tab is working properly.
                Owner = mainWindow
            };
            window.ShowDialog();
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
