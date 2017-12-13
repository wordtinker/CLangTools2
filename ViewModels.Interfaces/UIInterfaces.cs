﻿
namespace ViewModels.Interfaces
{
    // TODO check usage
    public interface IUIMainWindowService
    {
        bool SelectFolder(out string folderName);
        bool Confirm(string message);
        //void BeginInvoke(Action method);
        //string CommonDictionaryName { get; }
        //string AppDir { get; }
        bool OpenFile(string path);
        void ShowMessage(string message);
        void ManageLanguages();
        void ShowHelp();
        void Shutdown();
    }
}
