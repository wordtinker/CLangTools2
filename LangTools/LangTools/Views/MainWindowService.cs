using ViewModels.Interfaces;

namespace LangTools
{
    class MainWindowService : IUIMainWindowService
    {
        private MainWindow mainWindow;

        // TODO mainwindow or app?
        public MainWindowService(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }
        // TODO
    }
}
