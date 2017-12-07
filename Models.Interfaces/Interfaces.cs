using System;
using System.Collections.Generic;

namespace Models.Interfaces
{
    public enum ValidationError
    {
        LANGNAMEEMPTY,
        LANGWITHSPACES,
        LANGTAKEN,
        FOLDERNAMEEMPTY,
        FOLDERTAKEN,
        NONE
    }
    public interface IValidate
    {
        ValidationError ValidateLanguageName(string langName);
        ValidationError ValidateLanguageFolder(string langFolder);
    }
    public interface IFileStats
    {
        string FileName { get; }
        string FilePath { get; }
        string OutPath { get; }
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
    public interface IProject
    {
        string Name { get; set; }
    }
    public interface IDataProvider
    {
        IEnumerable<ILingva> GetLanguages();
        IEnumerable<IProject> GetProjects(ILingva lingva);
        IEnumerable<IDict> GetProjectDictionaries(IProject project);
        IEnumerable<IFileStats> GetProjectFiles(IProject project);
        IEnumerable<IFileStats> GetFilesWithWord(string word);
        IEnumerable<(string, int)> GetUnknownWords(IFileStats fileStats);
        IEnumerable<(string, int)> GetUnknownWords();
        void Analyze(IProject project, IProgress<(double Progress, IFileStats FileStats)> progress);
        ILingva CreateLanguage(string name, string folder);
        void RemoveLanguage(ILingva lingva);
    }
}
