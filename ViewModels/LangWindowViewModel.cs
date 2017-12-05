using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Models.Interfaces;
using ViewModels.Interfaces;
using System.Windows.Input;
using Prism.Commands;

namespace ViewModels
{
    public class LangWindowViewModel : BindableBase, IDataErrorInfo
    {
        //Members
        private MainViewModel mediatorVM;
        private IUIBaseService windowService;
        private IValidate validator;
        private ICommand getFolder;
        private ICommand addLanguage;
        private string language;
        private string folder;
        // Used for IDataErrorInfo
        private Dictionary<string, bool> validProperties = new Dictionary<string, bool>();
        private bool allPropertiesValid = false;

        // Properties
        public bool AllPropertiesValid
        {
            get => allPropertiesValid;
            set => SetProperty(ref allPropertiesValid, value);
        }
        public ObservableCollection<LingvaViewModel> Languages { get => mediatorVM.Languages; }
        public string Language
        {
            get => language;
            set => SetProperty(ref language, value);
        }
        public string Folder
        {
            get => folder;
            set => SetProperty(ref folder, value);
        }
        // ctor
        public LangWindowViewModel(MainViewModel mediatorVM, IUIBaseService windowService, IValidate validator)
        {
            this.mediatorVM = mediatorVM;
            this.windowService = windowService;
            this.validator = validator;
            validProperties.Add("Language", false);
            validProperties.Add("Folder", false);
        }
        // methods
        private void ValidateProperties()
        {
            foreach (bool isValid in validProperties.Values)
            {
                if (!isValid)
                {
                    AllPropertiesValid = false;
                    return;
                }
            }
            AllPropertiesValid = true;
        }

        // IDataErrorInfo Implementation
        public string this[string propertyName]
        {
            get
            {
                string error = null;
                switch (propertyName)
                {
                    case "Language":
                        switch (validator.ValidateLanguageName(Language))
                        {
                            case ValidationError.LANGNAMEEMPTY:
                                error = "Language name can't be empty.";
                                break;
                            case ValidationError.LANGTAKEN:
                                error = "Language is already in the database.";
                                break;
                            case ValidationError.LANGWITHSPACES:
                                error = "Language name contains trailing spaces.";
                                break;
                        }
                        break;
                    case "Folder":
                        switch (validator.ValidateLanguageFolder(Folder))
                        {
                            case ValidationError.FOLDERNAMEEMPTY:
                                error = "Select the folder.";
                                break;
                            case ValidationError.FOLDERTAKEN:
                                error = "Folder name is already taken.";
                                break;
                        }
                        break;
                    default:
                        throw new ApplicationException("Unknown Property being validated on Product.");
                }
                validProperties[propertyName] = String.IsNullOrEmpty(error);
                ValidateProperties();
                return error;
            }
        }
        public string Error => throw new NotImplementedException();
        // Commands
        public ICommand GetFolder
        {
            get
            {
                return getFolder ??
                (getFolder = new DelegateCommand(() =>
                {
                    if (windowService.SelectFolder(out string folderName))
                    {
                        // TODO
                        // Log.Logger.Debug(string.Format("Selected new folder for language: {0}", dirName));
                        Folder = folderName;
                    }
                }));
            }
        }
        public ICommand AddLanguage
        {
            get
            {
                return addLanguage ??
                (addLanguage = new DelegateCommand(() =>
                {
                    // TODO
                    // Log.Logger.Debug(string.Format("Requesting new language: {0}", dirName));
                    // Pass valid language into MainViewModel
                    mediatorVM.AddNewLanguage(Language, Folder);
                    // Clear text controls.
                    Language = string.Empty;
                    Folder = string.Empty;
                }));
            }
        }
    }
}
