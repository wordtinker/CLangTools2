using System;
using System.Collections.Generic;

namespace Models.Interfaces
{
    public interface IFileStats
    {
        string FileName { get; }
        int? Size { get; set; }
        int? Known { get; set; }
        int? Maybe { get; set; }
        int? Unknown { get; set; }
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
    // TODO document 
    public interface IDataProvider
    {
        IEnumerable<ILingva> GetLanguages();
        IEnumerable<string> GetProjects(ILingva lingva);
        IEnumerable<IDict> GetProjectDictionaries(string project);
        IEnumerable<IFileStats> GetProjectFiles(string project);
        IEnumerable<IFileStats> GetFilesWithWord(string word);
        // TODO 4.7 ()
        IEnumerable<Tuple<string, int>> GetUnknownWords(IFileStats fileStats);
        IEnumerable<Tuple<string, int>> GetUnknownWords();
    }
}
