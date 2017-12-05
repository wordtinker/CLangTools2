using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Models.Interfaces;
using ViewModels.Interfaces;

namespace ViewModels
{
    // TODO separate into class?
    public class LangWindowViewModel : BindableBase, IDataErrorInfo
    {
        //Members
        private MainViewModel mediatorVM;
        private IUIBaseService windowService;
        private IValidate validator;

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
        public string Language { get; set; }
        public string Folder { get; set; }
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
    }
}
