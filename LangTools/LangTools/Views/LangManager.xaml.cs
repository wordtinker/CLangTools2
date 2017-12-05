using System.Windows;

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
    }
}
