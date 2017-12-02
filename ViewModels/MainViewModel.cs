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
        private LingvaViewModel currentLingva;
        private bool projectSelectable = true; // defines if the user can switch project

        // Properties
        public ObservableCollection<LingvaViewModel> Languages { get; }
        public ObservableCollection<string> Projects { get; }
        public bool ProjectSelectable
        {
            get => projectSelectable;
            set => SetProperty(ref projectSelectable, value);
        }

        // ctor
        public MainViewModel(IUIMainWindowService windowService, IDataProvider dataProvider, ILoggerFacade logger)
        {
            this.windowService = windowService;
            this.dataProvider = dataProvider;
            this.logger = logger;
            Languages = new ObservableCollection<LingvaViewModel>();
            Projects = new ObservableCollection<string>();
            OnLoad();
        }
        // Methods
        private void OnLoad()
        {
            foreach (ILingva lang in dataProvider.GetLanguages())
            {
                Languages.Add(new LingvaViewModel(lang));
            }
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
            foreach (string project in dataProvider.GetProjectsForLanguage(lang.CurrentLanguage))
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
            // TODO STUB
            //// Remove old dictionaries
            //while (Dictionaries.Count > 0)
            //{
            //    Dictionaries.RemoveAt(0);
            //}
            //// Remove old FileStats
            //while (Files.Count > 0)
            //{
            //    Files.RemoveAt(0);
            //}
            //// Clear log and list of unknown words
            //Log = "";
            //WordsInProject.Clear();
        }
        /// <summary>
        /// Changes state of viewmodel according to selected project.
        /// </summary>
        /// <param name="item"></param>
        public void SelectProject(string project)
        {
            logger.Log("Project is selected.", Category.Debug, Priority.Medium);
            // TODO STUB
            //// ???
            //model.SelectProject(project);
            //// Update list of words related to project
            //ShowWordsForProject();
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
