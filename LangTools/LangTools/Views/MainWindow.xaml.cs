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

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Ensure that one of the languages is always selected
            if (languagesBox.SelectedIndex == -1)
            {
                // if there are no languages to select "set;" will be ignored
                // and wont raise new SelectionChanged Event.
                languagesBox.SelectedIndex = 0;
            }
        }
    }
}
