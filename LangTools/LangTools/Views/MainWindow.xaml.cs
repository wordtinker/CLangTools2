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
        /// Responds to DoubleClick even in the dataGrid with files.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilesRow_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGridRow row && row.DataContext is FileStatsViewModel fsvm)
            {
                // Let the view model open the output file.
                MainViewModel vm = (MainViewModel)base.DataContext;
                if (vm.OpenFile.CanExecute(null))
                {
                    vm.OpenFile.Execute(fsvm.OutPath);
                }
            }
        }
        /// <summary>
        /// Responds to doubleClick event in the dataGrid with dictionaries.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DictsRow_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGridRow row && row.DataContext is DictViewModel dvm)
            {
                // Let the view model open the dictionary.
                MainViewModel vm = (MainViewModel)base.DataContext;
                if (vm.OpenFile.CanExecute(null))
                {
                    vm.OpenFile.Execute(dvm.FilePath);
                }
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
                vm.AddWordToDictionary(wordVM);
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
