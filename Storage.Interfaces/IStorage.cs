using System.Collections.Generic;

namespace Storage.Interfaces
{
    public interface IStorage
    {
        IEnumerable<(string name, string folder)> GetLanguages();
        void AddLanguage(string name, string folder);
        void RemoveLanguage(string name);

        IEnumerable<string> GetProjects(string language);
        void RemoveProject(string language, string project);
        IEnumerable<(string name, string path, int size, int known,
            int maybe, int unknown)> GetFileStats(string language, string project);
        void RemoveFileStats(string filePath);
        void CommitStats(string name, string path, string lang, string project, int size, int known, int maybe, int unknown);
        void CommitWords(string filePath, IEnumerable<(string word, int count)> tokens);
        IEnumerable<(string, int)> GetUnknownWords(string filePath);
        IEnumerable<(string, int)> GetUnknownWords(string lang, string project);
        IEnumerable<string> GetFilesWithWord(string word);
        bool LanguageExists(string langName);
        bool FolderExists(string folderName);
    }
}
