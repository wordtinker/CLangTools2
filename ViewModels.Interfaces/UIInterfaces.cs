
namespace ViewModels.Interfaces
{
    public interface IUIMainWindowService
    {
        bool SelectFolder(out string folderName);
        bool Confirm(string message);
        bool OpenFile(string path);
        void ShowMessage(string message);
        void ManageLanguages();
        void ShowHelp();
        void Shutdown();
    }
}
