using System.Collections.Generic;

namespace Models.Interfaces
{
    public interface IFileStats
    {
        // TODO
    }
    public enum DictType
    {
        Project,
        General
    }
    public interface IDict
    {
        string FileName { get; set; }
        DictType DictType { get; set; }
        string FilePath { get; set; }
    }
    public interface ILingva
    {
        string Language { get; set; }
        string Folder { get; set; }
    }
    // TODO
    public interface IDataProvider
    {
        IEnumerable<ILingva> GetLanguages();
        IEnumerable<string> GetProjects(ILingva lingva);
        IEnumerable<IDict> GetProjectDictionaries(string project);
        IEnumerable<IFileStats> GetProjectFiles(string project);
    }
}
