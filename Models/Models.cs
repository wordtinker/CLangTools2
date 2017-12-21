using Core;
using Core.Interfaces;
using Models.Interfaces;
using Shared.Tools;
using Storage;
using Storage.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Models
{
    /// <summary>
    /// Configurational class that holds necessary
    /// settings for proper work.
    /// </summary>
    public static class Config
    {
        private static string commonDictionaryName = "Common";
        public static string CommonDictionaryName
        {
            get => commonDictionaryName;
            set => commonDictionaryName = string.IsNullOrWhiteSpace(value) ? commonDictionaryName : value;
        }
        public static string StyleDirectoryPath { get; set; } = null;
        public static string WorkingDirectory { get; set; } = null;
        public static readonly string DictExtension = ".dct";
        public static readonly string FileExtension = ".txt";
        public static readonly string OutExtension = ".html";
    }
    /// <summary>
    /// Container that provides model and validator classes for
    /// an app, and some internal classes.
    /// </summary>
    public static class ModelFactory
    {
        private static IStorage storage;
        private static IDataProvider dataProvider;
        private static IValidate validator;

        public static IDataProvider Model
        {
            get
            {
                return dataProvider ??
                    (dataProvider = new Model(Storage));
            }
        }
        public static IValidate Validator
        {
            get
            {
                return validator ??
                    (validator = new LingvaValidator(Storage));
            }
        }
        internal static ITreeBuilder TreeBuilder { get; } = new TreeBuilder();
        internal static ILexer Lexer
        {
            get => new Lexer();
        }
        internal static IStorage Storage
        {
            get
            {
                if (Config.WorkingDirectory == null) throw new ArgumentNullException("WorkingDirectory", "Working directory is not set");
                return storage ??
                    (storage = new SQLiteStorage(Config.WorkingDirectory));
            }
        }
    }
    /// <summary>
    /// Validator class.
    /// </summary>
    public class LingvaValidator : IValidate
    {
        private IStorage storage;
        internal LingvaValidator(IStorage storage)
        {
            this.storage = storage;
        }
        public ValidationError ValidateLanguageName(string langName)
        {
            if (langName == null) return ValidationError.LANGNAMEEMPTY;
            string lang = langName.Trim();
            if (lang.Length == 0) return ValidationError.LANGNAMEEMPTY;
            if (lang.Length != langName.Length) return ValidationError.LANGWITHSPACES;
            if (storage.LanguageExists(lang)) return ValidationError.LANGTAKEN;
            return ValidationError.NONE;
        }
        public ValidationError ValidateLanguageFolder(string langFolder)
        {
            if (langFolder == null) return ValidationError.FOLDERNAMEEMPTY;
            if (langFolder.Length == 0) return ValidationError.FOLDERNAMEEMPTY;
            if (storage.FolderExists(langFolder)) return ValidationError.FOLDERTAKEN;
            return ValidationError.NONE;
        }
    }

    public class FileStats : IFileStats
    {
        public string FileName { get; internal set; }
        public string FilePath { get; internal set; }
        public string OutPath { get; set; }

        public int? Size { get; set; }
        public int? Known { get; set; }
        public int? Maybe { get; set; }
        public int? Unknown => Size - Known - Maybe;
        // Override Equals
        public override bool Equals(object obj)
        {
            if (obj is FileStats other)
            {
                // valid filepath is never empty
                return FilePath.Equals(other.FilePath);
            }
            return false;
        }
        public override int GetHashCode()
        {
            // valid filepath is never empty
            return FilePath.GetHashCode();
        }
    }
    public class Dict : IDict
    {
        public string FileName { get; set; }
        public DictType DictType { get; set; }
        public string FilePath { get; set; }
        public override bool Equals(object obj)
        {
            if (obj is Dict other)
            {
                // Valid filepath is never empty
                return this.FilePath.Equals(other.FilePath);
            }
            return false;
        }
        public override int GetHashCode()
        {
            // Valid filepath is never empty
            return FilePath.GetHashCode();
        }
    }
    public class Lingva : ILingva
    {
        // Properties
        public string Language { get; set; }
        public string Folder { get; set; }
    }
    public class Project : IProject
    {
        public string Name { get; set; }
        public string Folder { get; set; }
        public ILingva Parent { get; set; }
    }

    public class Model : IDataProvider
    {   // Members
        private IStorage storage;
        // ctor
        internal Model(IStorage storage)
        {
            this.storage = storage;
        }
        // Methods
        public IEnumerable<ILingva> GetLanguages()
        {
            foreach (var (name, folder) in storage.GetLanguages())
            {
                yield return new Lingva { Language = name, Folder = folder };
            }
        }
        public ILingva CreateLanguage(string name, string folder)
        {
            storage.AddLanguage(name, folder);
            return new Lingva { Language = name, Folder = folder };
        }
        public void RemoveLanguage(ILingva lingva)
        {
            storage.RemoveLanguage(lingva.Language);
        }

        public IEnumerable<IProject> GetProjects(ILingva lingva)
        {
            // Take list of folders from lang directory
            if (IO.ListDirectories(lingva.Folder, out List<string> projectsInDir))
            {
                // Take list of projects from storage
                // Get known projects from DB
                var projectsInDB = storage.GetProjects(lingva.Language);
                // Find projects left only in DB
                foreach (string leftover in projectsInDB.Except(projectsInDir))
                {
                    // Remove leftover projects
                    storage.RemoveProject(lingva.Language, leftover);
                }
                // Return used projects
                foreach (string name in projectsInDir)
                {
                    if (IO.CombinePath(out string path, lingva.Folder, name))
                    {
                        yield return new Project { Name = name, Folder = path, Parent = lingva};
                    }
                }
            }
        }
        private IEnumerable<IFileStats> GetFiles(string folder)
        {
            // Get file names from project dir
            string filter = $"*{Config.FileExtension}";
            if (!IO.ListFiles(folder, out List<string> fileNames, filter: filter)) yield break;

            foreach (string fName in fileNames)
            {
                if (IO.CombinePath(out string path, folder, fName))
                {
                    yield return new FileStats
                    {
                        FileName = fName,
                        FilePath = path
                    };
                }
            }
        }
        private IEnumerable<IDict> GetDictionaries(string folder, DictType type)
        {
            string filter = $"*{Config.DictExtension}";
            if (!IO.ListFiles(folder, out List<string> dictionaries, filter: filter)) yield break;
            
            foreach (string fileName in dictionaries)
            {
                if (IO.CombinePath(out string path ,folder, fileName))
                {
                    yield return new Dict
                    {
                        DictType = type,
                        FileName = fileName,
                        FilePath = path
                    };
                }
            }
        }
        public IEnumerable<IDict> GetProjectDictionaries(IProject project)
        {
            // Get custom project dictionaries
            return GetDictionaries(project.Folder, DictType.Project)
            // Get general project dictionaries.
                .Concat(GetDictionaries(project.Parent.Folder, DictType.General));
        }
        public IEnumerable<IFileStats> GetProjectFiles(IProject project)
        {
            // Get list of filestats object in folder
            List<IFileStats> inDir = GetFiles(project.Folder).ToList();
            
            // Get list of objects from DB
            // Objects from DB shoul have output page already
            List<IFileStats> inDB = new List<IFileStats>();
            foreach (var (name, path, size, known, maybe, unknown) in
                storage.GetFileStats(project.Parent.Language, project.Name))
            {
                if (IO.ChangeExtension(path, Config.OutExtension, out string outPath))
                {
                    inDB.Add(new FileStats
                    {
                        FileName = name,
                        FilePath = path,
                        Size = size,
                        Known = known,
                        Maybe = maybe,
                        OutPath = outPath
                    });
                }
            }
            // Remove leftover stats from DB.
            foreach (IFileStats item in inDB.Except(inDir))
            {
                storage.RemoveFileStats(item.FilePath);
            }
            // NB: inDB.Intersect will return elements from inDB.
            // Need this order since they have more information.
            foreach (IFileStats item in inDB.Intersect(inDir))
            {
                yield return item;
            }
            // Add files that we have in dir but no stats in DB
            foreach (IFileStats item in inDir.Except(inDB))
            {
                yield return item;
            }
        }
        public IEnumerable<(string, int)> GetUnknownWords(IProject project)
        {
            return storage.GetUnknownWords(project.Parent.Language, project.Name);
        }
        public IEnumerable<(string, int)> GetUnknownWords(IFileStats fileStats)
        {
            return storage.GetUnknownWords(fileStats.FilePath);
        }

        public IEnumerable<IFileStats> GetFilesWithWord(string word)
        {
            foreach (string filePath in storage.GetFilesWithWord(word))
            {
                yield return new FileStats { FilePath = filePath };
            }
        }

        public void Analyze(IProject project, IProgress<(double Progress, IFileStats FileStats)> progress)
        {
            progress.Report((0d, null));
            // Remove old stats and words for project from DB.
            storage.RemoveProject(project.Parent.Language, project.Name);
            // Get file names and dic names from project dir
            var files = GetFiles(project.Folder);
            var dictionaries = GetProjectDictionaries(project);
            // Create object that handles analysis.
            Analyzer worker = new Analyzer(project, dictionaries);
            progress.Report((30d, null));
            double percentValue = 30;
            double step = 70.0 / files.Count();
            // Analyze files and report progress
            foreach (IFileStats fileStats in files)
            {
                percentValue += step;
                IFileStats newFileStats = worker.AnalyzeFile(fileStats);
                progress.Report((percentValue, newFileStats));
            }
            progress.Report((100d, null));
        }

        public bool DeleteFile(string path, out IFile file)
        {
            file = null;
            if (!IO.DeleteFile(path)) return false;

            if (path.EndsWith(Config.DictExtension))
            {
                file = new Dict { FilePath = path };
            }
            else if (path.EndsWith(Config.FileExtension))
            {
                file = new FileStats { FilePath = path };
                // Delete associated output file
                if (IO.ChangeExtension(path, Config.OutExtension, out string outPath))
                {
                    DeleteFile(outPath, out _);
                }
            }
            return true;
        }
        /// <summary>
        /// Appends the word to common dictionary.
        /// </summary>
        /// <param name="word"></param>
        public void AddWordToDictionary(IProject project, string word)
        {
            string wordToAppend = $"{word}{Environment.NewLine}";
            if (IO.CombinePath(out string path, project.Folder, Config.CommonDictionaryName) &&
                IO.ChangeExtension(path,Config.DictExtension, out path))
            {
                IO.AppendToFile(path, wordToAppend);
            }
        }
    }
}
