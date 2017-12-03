using System.Collections.Generic;
using Models.Interfaces;

namespace Models
{
    public class FileStats : IFileStats
    {

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
            yield return new FileStats { };
            yield return new FileStats { };
        }
    }
}
