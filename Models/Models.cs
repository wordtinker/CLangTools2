using System.Collections.Generic;
using Models.Interfaces;

namespace Models
{
    public static class ModelFactory
    {
        private static IDataProvider dataProvider;
        private static IValidate validator;

        public static IDataProvider Model
        {
            get
            {
                return dataProvider ??
                    (dataProvider = new StubModel());
            }
        }
        public static IValidate Validtor
        {
            get
            {
                return validator ??
                    (validator = new LingvaValidator());
            }
        }
    }

    public class LingvaValidator : IValidate
    {
        public ValidationError ValidateLanguageName(string langName)
        {
            if (langName == null) return ValidationError.LANGNAMEEMPTY;
            string lang = langName.Trim();
            if (lang.Length == 0) return ValidationError.LANGNAMEEMPTY;
            if (lang.Length != langName.Length) return ValidationError.LANGWITHSPACES;
            // TODO Later
            //if (dataProvider.LanguageExists(lang)) return ValidationError.LANGTAKEN;
            return ValidationError.NONE;
        }
        public ValidationError ValidateLanguageFolder(string langFolder)
        {
            if (langFolder == null) return ValidationError.FOLDERNAMEEMPTY;
            if (langFolder.Length == 0) return ValidationError.FOLDERNAMEEMPTY;
            // TODO Later
            //if (dataProvider.FolderExists(langFolder)) return ValidationError.FOLDERTAKEN;
            return ValidationError.NONE;
        }
    }

    public class FileStats : IFileStats
    {
        public string FileName { get; internal set; }
        public string FilePath { get; internal set; }
        public string OutPath { get; internal set; }

        public int? Size { get; set; }
        public int? Known { get; set; }
        public int? Maybe { get; set; }
        public int? Unknown { get; set; }
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
            yield return new Dict
            {
                DictType = DictType.General,
                FileName = "10000.txt",
                FilePath = @"C:\Users\Alex\Desktop\test\English\test.dct"
            };
            yield return new Dict { DictType = DictType.Project, FileName = project.Name, FilePath = "/test" };
        }
        public IEnumerable<IFileStats> GetProjectFiles(IProject project)
        {
            yield return new FileStats
            {
                FileName = "test.txt",
                FilePath = @"C:\Users\Alex\Desktop\test\English\test.txt",
                OutPath = @"C:\Users\Alex\Desktop\test\English\test.html",
                Size = 1000,
                Known = 900,
                Maybe = 50,
                Unknown = 50
            };
            yield return new FileStats
            {
                FileName = "test2.txt",
                FilePath = "test2.txt",
                Size = 15847,
                Known = 900,
                Maybe = 50,
                Unknown = 50
            };
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
            yield return new FileStats
            {
                FileName = "test.txt",
                FilePath = "test.txt",
                Size = 1000,
                Known = 900,
                Maybe = 50,
                Unknown = 50
            };
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

        public bool DeleteFile(string path, out IFile file)
        {
            file = new FileStats {FilePath = path };
            // Do deletion based on path.ext 
            return true;
        }
    }
}
