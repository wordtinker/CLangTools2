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
    public interface IFile
    {
        string FileName { get; }
        string FilePath { get; }
    }
    public interface IFileStats : IFile
    {
        string OutPath { get; }
        int? Size { get; set; }
        int? Known { get; set; }
        int? Maybe { get; set; }
        int? Unknown { get; }
    }
    public enum DictType
    {
        Project,
        General
    }
    public interface IDict : IFile
    {
        DictType DictType { get; set; }
    }
    public interface ILingva
    {
        string Language { get; set; }
        string Folder { get; set; }
    }
    public interface IProject
    {
        string Name { get; set; }
        string Folder { get; set; }
        ILingva Parent { get; set; }
    }
    public interface IDataProvider
    {
        IEnumerable<ILingva> GetLanguages();
        IEnumerable<IProject> GetProjects(ILingva lingva);
        IEnumerable<IDict> GetProjectDictionaries(IProject project);
        IEnumerable<IFileStats> GetProjectFiles(IProject project);
        IEnumerable<IFileStats> GetFilesWithWord(string word);
        IEnumerable<(string, int)> GetUnknownWords(IFileStats fileStats);
        IEnumerable<(string, int)> GetUnknownWords(IProject project);
        void Analyze(IProject project, IProgress<(double Progress, IFileStats FileStats)> progress);
        ILingva CreateLanguage(string name, string folder);
        void RemoveLanguage(ILingva lingva);
        /// <summary>
        /// Deletes named file. If passed file is a text file,
        /// deletes output file as well.
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <param name="file">Deleted file</param>
        /// <returns>True if file was deleted. out file is null if only output file was deleted
        /// or no deletion occured.
        /// If text file was deleted out file is IFileStats, if dictionary
        /// was deleted out file is IDict. </returns>
        bool DeleteFile(string path, out IFile file);
        void AddWordToDictionary(IProject project, string word);
    }
}
