using System;

namespace ViewModels.Interfaces
{
    // TODO check usage
    public interface IUIBaseService
    {
        bool SelectFolder(out string folderName);
        //bool Confirm(string message);
        //void BeginInvoke(Action method);
    }
    // TODO check usage
    public interface IUIMainWindowService : IUIBaseService
    {
        //string CommonDictionaryName { get; }
        //string AppDir { get; }
        //string AppName { get; }
        //string CorpusDir { get; }
        //string DicDir { get; }
        //string OutDir { get; }
        bool OpenFile(string path);
        void ShowMessage(string message);
        void ManageLanguages();
        void ShowHelp();
        void Shutdown();
    }
}
