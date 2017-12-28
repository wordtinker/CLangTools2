using Models.Interfaces;
using Prism.Events;
using Prism.Logging;
using System;
using System.Reflection;
using System.Windows;
using Unity;
using Unity.Resolution;
using ViewModels;
using ViewModels.Interfaces;

namespace LangTools
{
    class MainWindowService : IUIMainWindowService
    {
        private Window mainWindow;

        public MainWindowService(Window mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        public bool SelectFolder(out string folderName)
        {
            // Have to use windows forms.
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                folderName = dialog.SelectedPath;
                return true;
            }
            folderName = string.Empty;
            return false;
        }

        public void ManageLanguages()
        {
            LangManager window = new LangManager
            {
                // Ensure the alt+tab is working properly.
                Owner = mainWindow
            };
            var service = new MainWindowService(window);
            window.DataContext = App.Container.Resolve<LangWindowViewModel>(
                new ParameterOverride("windowService", service));
            window.ShowDialog();
        }
        public void ShowHelp()
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            MessageBox.Show($"LangTools: {version.ToString()}");
        }
        public void Shutdown()
        {
            App.Current.Shutdown();
        }

        /// <summary>
        /// Opens the file in the associated application.
        /// </summary>
        /// <param name="path"></param>
        public bool OpenFile(string path)
        {
            try
            {
                System.Diagnostics.Process.Start(path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public bool Confirm(string message)
        {
            MessageBoxResult result = MessageBox.Show(
                message,
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            return (result == MessageBoxResult.Yes);
        }
    }
}
