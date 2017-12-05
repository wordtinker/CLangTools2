﻿using System.Windows;

namespace LangTools
{
    /// <summary>
    /// Interaction logic for LangManager.xaml
    /// </summary>
    public partial class LangManager : Window
    {
        public LangManager()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Sends newly created language object to mainViewModel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddBtn_click(object sender, RoutedEventArgs e)
        {
            // TODO STUB
            //// Pass valid language into MainViewModel
            //LingvaViewModel lang = (LingvaViewModel)newLanguage.DataContext;
            //((MainViewModel)base.DataContext).AddNewLanguage(lang);
            //// Clear text controls.
            //langEdit.Clear();
            //folderEdit.Clear();
        }

        /// <summary>
        /// Asks mainViewModel to remove selected language.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveBtn_click(object sender, RoutedEventArgs e)
        {
            // TODO STUB
            //LingvaViewModel lang = languagesGrid.SelectedItem as LingvaViewModel;
            //if (lang != null)
            //{
            //    ((MainViewModel)base.DataContext).RemoveLanguage(lang);

            //}
        }

        /// <summary>
        /// Runs standard select Folder dialog, stores selected folder.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FolderBtn_click(object sender, RoutedEventArgs e)
        {
            // TODO STUB
            //// Have to use windows forms.
            //var dialog = new System.Windows.Forms.FolderBrowserDialog();
            //System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            //if (result == System.Windows.Forms.DialogResult.OK)
            //{
            //    string dirName = dialog.SelectedPath;

            //    Log.Logger.Debug(string.Format("Selected new folder for language: {0}", dirName));

            //    folderEdit.Text = dirName;
            //}
        }
    }
}
