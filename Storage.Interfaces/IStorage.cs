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
            int maybe, int unknown)> GetFilesStats(string language, string project);
        void RemoveFileStats(string filePath);
        //void UpdateStats(IFileStats stats);
        //void CommitStats();
        // TODO
        //void UpdateWords(string filePath, IEnumerable<TokenStats> tokens);
        //void CommitWords();
        IEnumerable<(string, int)> GetUnknownWords(string filePath);
        IEnumerable<(string, int)> GetUnknownWords(string lang, string project);
        IEnumerable<string> GetFilesWithWord(string word);
        bool LanguageExists(string langName);
        bool FolderExists(string folderName);
    }
}
