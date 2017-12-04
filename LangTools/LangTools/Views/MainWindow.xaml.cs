using Prism.Logging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ViewModels;

namespace LangTools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Constructor
        public MainWindow()
        {
            InitializeComponent();
            App.Logger.Log("MainWindow has started.", Category.Debug, Priority.Medium);
        }

        ///// <summary>
        ///// Shows modal window to manage languages.
        ///// </summary>
        private void LanguagesManage_click(object sender, RoutedEventArgs e)
        {
            // TODO STUB Skipped
            //LangWindow dialog = new LangWindow();
            //dialog.DataContext = base.DataContext;
            //// Ensure the alt+tab is working properly.
            //dialog.Owner = this;
            //dialog.ShowDialog();
        }

        /// <summary>
        /// Responds to DoubleClick even in the dataGrid with files.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilesRow_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            // TODO Later
            //DataGridRow row = (DataGridRow)sender;
            //// Let the view model open the report file.
            //((FileStatsViewModel)row.DataContext).OpenOutput();
        }

        /// <summary>
        /// Responds to click event in the context menu of the dataGrid with files.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilesContextMenu_ClickOpenFile(object sender, RoutedEventArgs e)
        {
            if (filesGrid.SelectedItem is FileStatsViewModel item)
            {
                // Let the view model open origin file.
                // TODO Later
                //item.OpenFile();
            }
        }

        /// <summary>
        /// Responds to click event in the context menu of the dataGrid with files.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilesContextMenu_ClickOpenOutput(object sender, RoutedEventArgs e)
        {
            if (filesGrid.SelectedItem is FileStatsViewModel item)
            {
                // Let the view model open report file.
                // TODO Later
                //item.OpenOutput();
            }
        }

        /// <summary>
        /// Responds to click event in the context menu of the dataGrid with files.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilesContextMenu_ClickDeleteFile(object sender, RoutedEventArgs e)
        {
            if (filesGrid.SelectedItem is FileStatsViewModel item)
            {
                // Let the view model delete the origin file.
                // TODO Later
                //item.DeleteFile();
            }
        }

        /// <summary>
        /// Responds to click event in the context menu of the dataGrid with files.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilesContextMenu_ClickDeleteOutput(object sender, RoutedEventArgs e)
        {
            if (filesGrid.SelectedItem is FileStatsViewModel item)
            {
                // Let the view model delete the report file.
                // TODO Later
                //item.DeleteOutput();
            }
        }

        /// <summary>
        /// Responds to doubleClick event in the dataGrid with dictionaries.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DictsRow_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            // TODO Later
            //DataGridRow row = (DataGridRow)sender;
            //// Let the view model open the dictionary.
            //((DictViewModel)row.DataContext).OpenFile();
        }

        /// <summary>
        /// Responds to click event in the context menu of the dataGrid with dictionaries.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DictContextMenu_ClickOpen(object sender, RoutedEventArgs e)
        {
            if (dictsGrid.SelectedItem is DictViewModel item)
            {
                // Let the view model open the dictionary.
                // TODO Later as baseclass with open methods?
                //item.OpenFile();
            }
        }

        /// <summary>
        /// Responds to click event in the context menu of the dataGrid with dictionaries.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DictContextMenu_ClickDelete(object sender, RoutedEventArgs e)
        {
            if (dictsGrid.SelectedItem is DictViewModel item)
            {
                // Let the view model delete the dicionary.
                // TODO Later
                //item.DeleteFile();
            }
        }

        /// <summary>
        /// Responds to doubleClick event in the dataGrid with words.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WordRow_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            MainViewModel vm = (MainViewModel)base.DataContext;
            if (sender is DataGridRow item && item.DataContext is WordViewModel wordVM)
            {
                // Let view model add the word into dictionary.
                // TODO Later
                //vm.AddWordToDictionary(wordVM);
            }
        }

        /// <summary>
        /// Responds to MouseDown event in the dataGrid with words.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WordRow_SingleClick(object sender, MouseButtonEventArgs e)
        {
            MainViewModel vm = (MainViewModel)base.DataContext;
            if (sender is DataGridRow item && item.DataContext is WordViewModel wordVM)
            {
                // Let view model mark files to highlight.
                vm.HighlightFilesWithWord(wordVM);
            }
        }
    }
}
