using Prism.Commands;
using Prism.Mvvm;
using Prism.Logging;
using Models.Interfaces;
using ViewModels.Interfaces;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace ViewModels
{
    public class MainViewModel : BindableBase
    {
        // Members
        private IUIMainWindowService windowService;
        private IDataProvider dataProvider;
        private ILoggerFacade logger;
        private ICommand exitApp;
        private ICommand showHelp;
        private bool projectSelectable = true; // defines if the user can switch project
        private string log;
        private int progressValue;

        // Properties
        public ObservableCollection<LingvaViewModel> Languages { get; }
        public ObservableCollection<string> Projects { get; }
        public ObservableCollection<DictViewModel> Dictionaries { get; }
        public ObservableCollection<FileStatsViewModel> Files { get; }
        public ObservableCollection<WordViewModel> Words { get; }
        public ObservableCollection<WordViewModel> WordsInProject { get; }
        public bool ProjectSelectable
        {
            get => projectSelectable;
            set => SetProperty(ref projectSelectable, value);
        }
        public string Log
        {
            get => log;
            set => SetProperty(ref log, value);
        }
        public int ProgressValue
        {
            get => progressValue;
            set => SetProperty(ref progressValue, value);
        }

        // ctor
        public MainViewModel(IUIMainWindowService windowService, IDataProvider dataProvider, ILoggerFacade logger)
        {
            this.windowService = windowService;
            this.dataProvider = dataProvider;
            this.logger = logger;
            Languages = new ObservableCollection<LingvaViewModel>();
            Projects = new ObservableCollection<string>();
            Dictionaries = new ObservableCollection<DictViewModel>();
            Files = new ObservableCollection<FileStatsViewModel>();
            Words = new ObservableCollection<WordViewModel>();
            WordsInProject = new ObservableCollection<WordViewModel>();
            OnLoad();
            logger.Log("MainView has started.", Category.Debug, Priority.Medium);
        }
        // Methods
        private void OnLoad()
        {
            foreach (ILingva lang in dataProvider.GetLanguages())
            {
                Languages.Add(new LingvaViewModel(lang));
            }
            ProgressValue = 100;
        }
        /// <summary>
        /// Prepares modelView for new language.
        /// </summary>
        public void LanguageIsAboutToChange()
        {
            logger.Log("Language is about to change.", Category.Debug, Priority.Medium);
            // Clear list of projects
            Projects.Clear();
        }
        /// <summary>
        /// Changes state of viewModel according to selected language.
        /// </summary>
        /// <param name="item"></param>
        public void SelectLanguage(LingvaViewModel lang)
        {
            logger.Log("Language is selected.", Category.Debug, Priority.Medium);
            // Ask model for new list of projects
            foreach (string project in dataProvider.GetProjects(lang.CurrentLanguage))
            {
                Projects.Add(project);
            }
        }
        /// <summary>
        /// Prepares viewModel for new project.
        /// </summary>
        public void ProjectIsAboutToChange()
        {
            logger.Log("Project is about to change.", Category.Debug, Priority.Medium);
            // Clear dictionaries for previous project
            Dictionaries.Clear();
            // Clear FileStats
            Files.Clear();
            // Clear log and list of unknown words
            Log = "";
            WordsInProject.Clear();
            // TODO Test ???
            Words.Clear();
        }
        /// <summary>
        /// Changes state of viewmodel according to selected project.
        /// </summary>
        /// <param name="item"></param>
        public void SelectProject(string project)
        {
            logger.Log("Project is selected.", Category.Debug, Priority.Medium);
            // TODO Test
            // language might be null during the proccess
            // of changing language.
            //if (currentLanguage == null) return;

            // Get both custom and general project dictionaries
            foreach (IDict dict in dataProvider.GetProjectDictionaries(project))
            {
                Dictionaries.Add(new DictViewModel(dict));
            }
            // Get files for a project
            foreach (IFileStats file in dataProvider.GetProjectFiles(project))
            {
                Files.Add(new FileStatsViewModel(file));
            }
            // Update list of words related to project
            LoadWordsForProject();
        }
        /// <summary>
        /// Fills the words table for the whole project.
        /// </summary>
        private void LoadWordsForProject()
        {
            // TODO STUB
            //foreach (var item in model.GetUnknownWords())
            //{
            //    WordsInProject.Add(new WordViewModel { Word = item.Key, Quantity = item.Value });
            //}
        }
        // Commands
        public ICommand ExitApp
        {
            get
            {
                return exitApp ??
                (exitApp = new DelegateCommand(() =>
                {
                    windowService.Shutdown();
                }));
            }
        }
        public ICommand ShowHelp
        {
            get
            {
                return showHelp ??
                (showHelp = new DelegateCommand(() =>
                {
                    windowService.ShowHelp();
                }));
            }
        }
    }
}
