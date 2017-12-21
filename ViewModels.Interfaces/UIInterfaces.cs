
namespace ViewModels.Interfaces
{
    /// <summary>
    /// Methods that service must provide for viewmodel
    /// to work properly.
    /// </summary>
    public interface IUIMainWindowService
    {
        /// <summary>
        /// Provide a string of selected folder.
        /// </summary>
        bool SelectFolder(out string folderName);
        /// <summary>
        /// Ask user to confirm action.
        /// </summary>
        bool Confirm(string message);
        /// <summary>
        /// Open file with default app.
        /// </summary>
        bool OpenFile(string path);
        /// <summary>
        /// Show user a message.
        /// </summary>
        void ShowMessage(string message);
        /// <summary>
        /// Open GUI for managing languages.
        /// </summary>
        void ManageLanguages();
        /// <summary>
        /// Show help.
        /// </summary>
        void ShowHelp();
        /// <summary>
        /// Shut down app.
        /// </summary>
        void Shutdown();
    }
}
