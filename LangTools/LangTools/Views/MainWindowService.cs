using System.Windows;
using ViewModels.Interfaces;
using ViewModels;
using Models;
using System;

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
            LangWindowViewModel vm = new LangWindowViewModel(
                (MainViewModel)mainWindow.DataContext, this, ModelFactory.Validtor,
                ModelFactory.Model, App.Logger);
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
    }
}
