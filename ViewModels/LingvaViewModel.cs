using Prism.Mvvm;
using Models.Interfaces;
using System;

namespace ViewModels
{
    /// <summary>
    /// Represents language.
    /// </summary>
    public class LingvaViewModel : BindableBase
    {
        // Members
        private readonly ILingva currentLanguage;
        // Used for IDataErrorInfo
        // TODO add errorinfo interface
        //private Dictionary<string, bool> validProperties = new Dictionary<string, bool>();
        //private bool allPropertiesValid = false;

        // Properties
        public ILingva Lingva
        {
            get => currentLanguage;
        }

        public string Language
        {
            get => currentLanguage.Language;
            // TODO do we ever set?
            //set => SetProperty(ref currentLanguage.Language, value);
        }

        public string Folder
        {
            get => currentLanguage.Folder;
            // TODO do we ever set?
            //set => SetProperty(currentLanguage.Folder, value);
        }

        // TODO
        //public bool AllPropertiesValid
        //{
        //    get { return allPropertiesValid; }
        //    set
        //    {
        //        if (allPropertiesValid != value)
        //        {
        //            allPropertiesValid = value;
        //            RaisePropertyChanged();
        //        }
        //    }
        //}

        // Constructors
        public LingvaViewModel(ILingva language)
        {
            currentLanguage = language;
            // TODO
            //validProperties.Add("Language", false);
            //validProperties.Add("Folder", false);
        }

        // Methods
        // TODO
        //private void ValidateProperties()
        //{
        //    foreach (bool isValid in validProperties.Values)
        //    {
        //        if (!isValid)
        //        {
        //            AllPropertiesValid = false;
        //            return;
        //        }
        //    }
        //    AllPropertiesValid = true;
        //}

        // Equals override
        public override bool Equals(object obj)
        {
            LingvaViewModel item = obj as LingvaViewModel;
            if (item == null)
            {
                return false;
            }
            return this.currentLanguage.Equals(item.currentLanguage);
        }

        public override int GetHashCode()
        {
            return currentLanguage.GetHashCode();
        }

        // DataErrorInfo interface
        // TODO
        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        // TODO syntax?
        //public string this[string propertyName]
        //{
        //    get
        //    {
        //        string error = null;
        //        switch (propertyName)
        //        {
        //            case "Language":
        //                switch (currentLanguage.ValidateLanguageName())
        //                {
        //                    case ValidationError.LANGNAMEEMPTY:
        //                        error = "Language name can't be empty.";
        //                        break;
        //                    case ValidationError.LANGTAKEN:
        //                        error = "Language is already in the database.";
        //                        break;
        //                    case ValidationError.LANGWITHSPACES:
        //                        error = "Language name contains trailing spaces.";
        //                        break;
        //                }
        //                break;
        //            case "Folder":
        //                switch (currentLanguage.ValidateLanguageFolder())
        //                {
        //                    case ValidationError.FOLDERNAMEEMPTY:
        //                        error = "Select the folder.";
        //                        break;
        //                    case ValidationError.FOLDERTAKEN:
        //                        error = "Folder name is already taken.";
        //                        break;
        //                }
        //                break;
        //            default:
        //                throw new ApplicationException("Unknown Property being validated on Product.");
        //        }

        //        validProperties[propertyName] = String.IsNullOrEmpty(error);
        //        ValidateProperties();
        //        return error;
        //    }
        //}
    }
}
