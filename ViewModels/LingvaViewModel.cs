using Prism.Mvvm;
using Models.Interfaces;

namespace ViewModels
{
    /// <summary>
    /// Represents language.
    /// </summary>
    public class LingvaViewModel : BindableBase
    {
        // Members
        private readonly ILingva currentLanguage;
        // Properties
        public ILingva Lingva { get => currentLanguage; }
        public string Language { get => currentLanguage.Language; }
        public string Folder { get => currentLanguage.Folder; }
        // Constructor
        public LingvaViewModel(ILingva language)
        {
            currentLanguage = language;
        }
    }
}
