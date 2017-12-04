using Prism.Commands;
using Prism.Mvvm;
using Prism.Logging;
using Models.Interfaces;
using ViewModels.Interfaces;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using System;

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
        private ICommand runProject;
        private FileStatsViewModel currentFile; // currently selected file
        private ProjectViewModel currentProject;
        private bool projectSelectable = true; // defines if the user can switch project
        private string log;
        private int progressValue;
        private int totalWords; // total words in the project files
        private int totalUnknown; // unknown words in the project files

        // Properties
        public ObservableCollection<LingvaViewModel> Languages { get; }
        public ObservableCollection<ProjectViewModel> Projects { get; }
        public ObservableCollection<DictViewModel> Dictionaries { get; }
        public ObservableCollection<FileStatsViewModel> Files { get; }
        public ObservableCollection<WordViewModel> Words { get; }
        public ObservableCollection<WordViewModel> WordsInProject { get; }
        public bool ReadyToRun
        {
            get => projectSelectable;
            set => SetProperty(ref projectSelectable, value);
        }
        public bool CanRunAnalysis
        {
            get
            {
                return Languages.Count > 0
                    && Projects.Count > 0
                    && Files.Count > 0
                    && ReadyToRun;
            }
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
        public int TotalWords
        {
            get => totalWords;
            set
            {
                if (SetProperty(ref totalWords, value))
                {
                    RaisePropertyChanged(nameof(UnknownPercent));
                }
            }
        }
        public double UnknownPercent
        {
            get => totalWords == 0 ? 0 : (double)totalUnknown / totalWords;
        }

        // ctor
        public MainViewModel(IUIMainWindowService windowService, IDataProvider dataProvider, ILoggerFacade logger)
        {
            this.windowService = windowService;
            this.dataProvider = dataProvider;
            this.logger = logger;
            Languages = new ObservableCollection<LingvaViewModel>();
            Projects = new ObservableCollection<ProjectViewModel>();
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
            foreach (IProject project in dataProvider.GetProjects(lang.CurrentLanguage))
            {
                Projects.Add(new ProjectViewModel(project));
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
            Words.Clear();
        }
        /// <summary>
        /// Changes state of viewmodel according to selected project.
        /// </summary>
        /// <param name="item"></param>
        public void SelectProject(ProjectViewModel project)
        {
            logger.Log("Project is selected.", Category.Debug, Priority.Medium);
            // TODO Later redo currproject binding
            currentProject = project;
            // Get both custom and general project dictionaries
            foreach (IDict dict in dataProvider.GetProjectDictionaries(project.Project))
            {
                Dictionaries.Add(new DictViewModel(dict));
            }
            // Get files for a project
            foreach (IFileStats file in dataProvider.GetProjectFiles(project.Project))
            {
                Files.Add(new FileStatsViewModel(file));
            }
            // Update list of words related to project
            LoadWordsForProject();
        }
        /// <summary>
        /// Prepares viewModel for new file selection.
        /// </summary>
        public void FileRowIsAboutToChange()
        {
            currentFile = null;
            Words.Clear();
        }
        /// <summary>
        /// Fills the Words table with words.
        /// </summary>
        /// <param name="fileStatsVM"></param>
        public void LoadWords(FileStatsViewModel fileStatsVM)
        {
            currentFile = fileStatsVM;
            if (!ReadyToRun) { return; }
            foreach ((string word, int quantity) in dataProvider.GetUnknownWords(fileStatsVM.FileStats))
            {
                Words.Add(new WordViewModel { Word = word, Quantity = quantity });
            }
        }
        /// <summary>
        /// Recalculates the stats of the whole project.
        /// </summary>
        private void UpdateTotalStats()
        {
            totalUnknown = Files.Sum(x => x.Unknown.GetValueOrDefault());
            TotalWords = Files.Sum(x => x.Size.GetValueOrDefault());
        }
        /// <summary>
        /// Fills the words table for the whole project.
        /// </summary>
        private void LoadWordsForProject()
        {
            foreach ((string word, int quantity) in dataProvider.GetUnknownWords())
            {
                WordsInProject.Add(new WordViewModel { Word = word, Quantity = quantity });
            }
        }
        /// <summary>
        /// Removes highligting from every file.
        /// </summary>
        public void RemoveHighlighting()
        {
            foreach (FileStatsViewModel file in Files.Where(i => i.Highlighted))
            {
                file.Highlighted = false;
            }
        }
        /// <summary>
        /// Starts the analysis of the project files.
        /// </summary>
        /// <returns></returns>
        private async Task HandleAnalysis()
        {
            // Prevent changing of the project.
            ReadyToRun = false;
            // Clear old project data.
            Words.Clear();
            WordsInProject.Clear();
            RemoveHighlighting();

            // Get the old project stats
            int oldKnownQty = Files.Sum(x => x.Known.GetValueOrDefault());
            int oldMaybeQty = Files.Sum(x => x.Maybe.GetValueOrDefault());
            ProgressValue = 0;
            logger.Log("Requesting Project analysis.", Category.Debug, Priority.Medium);
            // Start analysis
            await Task.Run(() => dataProvider.Analyze(
                currentProject.Project,
                new Progress<(double Progress, IFileStats FileStats)>(p =>
                {
                    //Update the visual progress of the analysis.
                    ProgressValue = Convert.ToInt32(p.Progress);
                    if (p.FileStats != null)
                    {
                        // Updating stats
                        var foundFile = Files.Where(fsvm => fsvm.FileStats.Equals(p.FileStats)).FirstOrDefault();
                        if (foundFile != null)
                        {
                            foundFile.Update(p.FileStats);
                            Log = string.Format("{0} is ready!", foundFile.FileName);
                        }
                    }
                }
                )));
            // Get new project stats
            int newKnownQty = Files.Sum(x => x.Known.GetValueOrDefault());
            int newMaybeQty = Files.Sum(x => x.Maybe.GetValueOrDefault());
            // Update the visual progress.
            ProgressValue = 100;
            Log = string.Format(
                "Analysis is finished. Known: {0:+#;-#;0}, Maybe {1:+#;-#;0}", // Force sign, no sign for zero
                newKnownQty - oldKnownQty, newMaybeQty - oldMaybeQty);
            logger.Log("Project analysis is ready.", Category.Debug, Priority.Medium);
            // Update totals
            UpdateTotalStats();
            ReadyToRun = true;
            // Update WordList
            if (currentFile != null) { LoadWords(currentFile); }
            LoadWordsForProject();
        }
        /// <summary>
        /// Marks the files that contain the word.
        /// </summary>
        /// <param name="word"></param>
        public void HighlightFilesWithWord(WordViewModel word)
        {
            // Remove previously highlighted files
            RemoveHighlighting();
            // Request IFileStats containig the word and compare with existing files
            // Intersect must be called on Files
            var query = Files.Intersect(
                dataProvider.GetFilesWithWord(word.Word).Select(fs => new FileStatsViewModel(fs))
                );
            // Highlight selected files
            foreach (var item in query)
            {
                item.Highlighted = true;
            }
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
        public ICommand RunProject
        {
            get
            {
                return runProject ??
                (runProject = new DelegateCommand(
                    async () => await HandleAnalysis())
                    .ObservesProperty(() => Languages)
                    .ObservesProperty(() => Projects)
                    .ObservesProperty(() => Files)
                    .ObservesProperty(() => ReadyToRun)
                    .ObservesCanExecute(() => CanRunAnalysis)
                );
            }
        }
    }
}
