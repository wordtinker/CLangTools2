using System;
using System.Collections.Generic;
using Models.Interfaces;

namespace Models
{
    public class FileStats : IFileStats
    {
        public string FileName { get; internal set; }
        public int? Size { get; set; }
        public int? Known { get; set; }
        public int? Maybe { get; set; }
        public int? Unknown { get; set; }
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

    public class StubModel : IDataProvider
    {
        // TODO STUB
        public IEnumerable<ILingva> GetLanguages()
        {
            yield return new Lingva { Language = "English", Folder = "/test" };
            yield return new Lingva { Language = "Hebrew", Folder = "/test" };
        }
        public IEnumerable<string> GetProjects(ILingva lingva)
        {
            yield return "books" + lingva.Language;
            yield return "sdfsdf" + lingva.Language;
        }
        public IEnumerable<IDict> GetProjectDictionaries(string project)
        {
            yield return new Dict { DictType = DictType.General, FileName = "10000.txt", FilePath = "/test" };
            yield return new Dict { DictType = DictType.Project, FileName = project, FilePath = "/test" };
        }
        public IEnumerable<IFileStats> GetProjectFiles(string project)
        {
            yield return new FileStats {FileName = "test.txt", Size = 1000, Known = 900, Maybe = 50, Unknown = 50 };
            yield return new FileStats { };
        }

        public IEnumerable<Tuple<string, int>> GetUnknownWords(IFileStats fileStats)
        {
            yield return Tuple.Create("word", 54);
            yield return Tuple.Create("word2", 2);
        }
        public IEnumerable<Tuple<string, int>> GetUnknownWords()
        {
            yield return Tuple.Create("word", 154);
            yield return Tuple.Create("word2", 12);
        }

        public IEnumerable<IFileStats> GetFilesWithWord(string word)
        {
            yield return new FileStats { FileName = "test.txt", Size = 1000, Known = 900, Maybe = 50, Unknown = 50 };
        }
    }
}
