using System;
using System.Collections.Generic;

namespace Models.Interfaces
{
    /// <summary>
    /// Validation error types for new language validation.
    /// </summary>
    public enum ValidationError
    {
        LANGNAMEEMPTY,
        LANGWITHSPACES,
        LANGTAKEN,
        FOLDERNAMEEMPTY,
        FOLDERTAKEN,
        NONE
    }
    /// <summary>
    /// Validates two parts of new language.
    /// </summary>
    public interface IValidate
    {
        ValidationError ValidateLanguageName(string langName);
        ValidationError ValidateLanguageFolder(string langFolder);
    }
    /// <summary>
    /// File representation.
    /// </summary>
    public interface IFile
    {
        string FileName { get; }
        string FilePath { get; }
    }
    /// <summary>
    /// Text file representation.
    /// </summary>
    public interface IFileStats : IFile
    {
        string OutPath { get; set; }
        int? Size { get; set; }
        int? Known { get; set; }
        int? Maybe { get; set; }
        int? Unknown { get; }
    }
    /// <summary>
    /// Types of dictionary, project specific or general -
    /// covering every project.
    /// </summary>
    public enum DictType
    {
        Project,
        General
    }
    /// <summary>
    /// Dictionary file representation.
    /// </summary>
    public interface IDict : IFile
    {
        DictType DictType { get; set; }
    }
    /// <summary>
    /// Language representation.
    /// </summary>
    public interface ILingva
    {
        string Language { get; set; }
        string Folder { get; set; }
    }
    /// <summary>
    /// Project represenatation.
    /// </summary>
    public interface IProject
    {
        string Name { get; set; }
        string Folder { get; set; }
        ILingva Parent { get; set; }
    }
    /// <summary>
    /// Provides all operations of the model.
    /// </summary>
    public interface IDataProvider
    {
        /// <summary>
        /// Provides list of languages stored in the model.
        /// </summary>
        /// <returns></returns>
        IEnumerable<ILingva> GetLanguages();
        /// <summary>
        /// Provides list of projects stored(list of project folders) in the model for
        /// specific language.
        /// </summary>
        /// <param name="lingva"></param>
        /// <returns></returns>
        IEnumerable<IProject> GetProjects(ILingva lingva);
        /// <summary>
        /// Provides list of dictionaries of both types related to given project.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        IEnumerable<IDict> GetProjectDictionaries(IProject project);
        /// <summary>
        /// Provides list of text files constituting given project.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        IEnumerable<IFileStats> GetProjectFiles(IProject project);
        /// <summary>
        /// Provides list of text files that have given word.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        IEnumerable<IFileStats> GetFilesWithWord(string word);
        /// <summary>
        /// Provides list of words that has been marked as unknown for a given text file.
        /// </summary>
        /// <param name="fileStats"></param>
        /// <returns></returns>
        IEnumerable<(string, int)> GetUnknownWords(IFileStats fileStats);
        /// <summary>
        /// Provides list of words that has been marked as unknown for a given project.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        IEnumerable<(string, int)> GetUnknownWords(IProject project);
        /// <summary>
        /// Makes analysis of given project and provides updated stats for every text
        /// file in the project.
        /// </summary>
        /// <param name="project"></param>
        /// <param name="progress"></param>
        void Analyze(IProject project, IProgress<(double Progress, IFileStats FileStats)> progress);
        /// <summary>
        /// Saves a new language in the model.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        ILingva CreateLanguage(string name, string folder);
        /// <summary>
        /// Removes language from the model.
        /// </summary>
        /// <param name="lingva"></param>
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
        /// <summary>
        /// Adds given word to special dictionary related to given project.
        /// </summary>
        /// <param name="project"></param>
        /// <param name="word"></param>
        void AddWordToDictionary(IProject project, string word);
    }
}
