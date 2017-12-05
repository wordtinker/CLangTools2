using System.Collections.Generic;
using Models.Interfaces;

namespace Models
{
    public class LingvaValidator : IValidate
    {
        public ValidationError ValidateLanguageName(string langName)
        {
            if (langName == null) return ValidationError.LANGNAMEEMPTY;
            string lang = langName.Trim();
            if (lang.Length == 0) return ValidationError.LANGNAMEEMPTY;
            if (lang.Length != langName.Length) return ValidationError.LANGWITHSPACES;
            // TODO
            //if (dataProvider.LanguageExists(lang)) return ValidationError.LANGTAKEN;
            return ValidationError.NONE;
        }
        public ValidationError ValidateLanguageFolder(string langFolder)
        {
            if (langFolder == null) return ValidationError.FOLDERNAMEEMPTY;
            if (langFolder.Length == 0) return ValidationError.FOLDERNAMEEMPTY;
            // TODO
            //if (dataProvider.FolderExists(langFolder)) return ValidationError.FOLDERTAKEN;
            return ValidationError.NONE;
        }
    }

    public class FileStats : IFileStats
    {
        // TODO must implement Equals
        public string FileName { get; internal set; }
        public string FilePath { get; internal set; }

        public int? Size { get; set; }
        public int? Known { get; set; }
        public int? Maybe { get; set; }
        public int? Unknown { get; set; }
        // Overrided Equals
        public override bool Equals(object obj)
        {
            if (obj is FileStats item)
            {
                string baseLine = FilePath ?? string.Empty;
                return baseLine.Equals(item.FilePath);
            }
            return false;
        }
        public override int GetHashCode()
        {
            string baseLine = FilePath ?? string.Empty;
            // TODO hash of empty string
            return FilePath.GetHashCode();
        }
    }
    public class Dict : IDict
    {
        public string FileName { get; set; }
        public DictType DictType { get; set; }
        public string FilePath { get; set; }
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
    }

    public class StubModel : IDataProvider
    {
        public IEnumerable<ILingva> GetLanguages()
        {
            yield return new Lingva { Language = "English", Folder = "/test" };
            yield return new Lingva { Language = "Hebrew", Folder = "/test" };
        }
        public IEnumerable<IProject> GetProjects(ILingva lingva)
        {
            yield return new Project { Name = "books" + lingva.Language };
            yield return new Project { Name = "other" + lingva.Language };
        }
        public IEnumerable<IDict> GetProjectDictionaries(IProject project)
        {
            yield return new Dict { DictType = DictType.General, FileName = "10000.txt", FilePath = "/test" };
            yield return new Dict { DictType = DictType.Project, FileName = project.Name, FilePath = "/test" };
        }
        public IEnumerable<IFileStats> GetProjectFiles(IProject project)
        {
            yield return new FileStats {
                FileName = "test.txt",
                FilePath = "test.txt",
                Size = 1000,
                Known = 900,
                Maybe = 50,
                Unknown = 50 };
            yield return new FileStats { };
        }

        public IEnumerable<(string, int)> GetUnknownWords(IFileStats fileStats)
        {
            yield return ("word", 54);
            yield return ("word2", 2);
        }
        public IEnumerable<(string, int)> GetUnknownWords()
        {
            yield return ("word", 154);
            yield return ("word2", 12);
        }

        public IEnumerable<IFileStats> GetFilesWithWord(string word)
        {
            yield return new FileStats { FileName = "test.txt", Size = 1000, Known = 900, Maybe = 50, Unknown = 50 };
        }

        public void Analyze(IProject project, System.IProgress<(double Progress, IFileStats FileStats)> progress)
        {
            progress.Report((0, null));
            progress.Report((10.54, new FileStats {
                FileName = "test.txt",
                FilePath = "test.txt",
                Size = 9999,
                Known = 900,
                Maybe = 50,
                Unknown = 5000 }));
        }

        public ILingva CreateLanguage(string name, string folder)
        {
            return new Lingva { Language = name, Folder = folder };
        }

        public void RemoveLanguage(ILingva lingva)
        {
            // DO nothing
        }
    }
}
