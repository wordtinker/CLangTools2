using Storage.Interfaces;
using System.Collections.Generic;

namespace Storage
{
    public class StubStorage : IStorage
    {
        public StubStorage(string directory)
        {

        }
        public void AddLanguage(string name, string folder)
        {
            // do nothing
        }

        public bool FolderExists(string folderName)
        {
            return false;
        }

        public IEnumerable<(string name, string path, int size, int known, int maybe, int unknown)> GetFileStats(string language, string project)
        {
            yield break;
        }

        public IEnumerable<string> GetFilesWithWord(string word)
        {
            yield break;   
        }

        public IEnumerable<(string name, string folder)> GetLanguages()
        {
            yield break;
        }

        public IEnumerable<string> GetProjects(string language)
        {
            yield break;
        }

        public IEnumerable<(string, int)> GetUnknownWords(string filePath)
        {
            yield break;
        }

        public IEnumerable<(string, int)> GetUnknownWords(string lang, string project)
        {
            yield break;
        }

        public bool LanguageExists(string langName)
        {
            return false;
        }

        public void RemoveFileStats(string path)
        {
            // Do nothing
        }

        public void RemoveLanguage(string name)
        {
            // do nothing
            // TODO NB only lang, cascade?
        }

        public void RemoveProject(string language, string project)
        {
            // do nothing
            // TODO NB only project, cascade?
        }
    }
}
