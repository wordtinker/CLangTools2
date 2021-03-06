﻿using Models.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Logging;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using ViewModels.Events;
using ViewModels.Interfaces;

namespace ViewModels
{
    /// <summary>
    /// Part of the class that implements IDataErrorInfo
    /// </summary>
    public partial class LangWindowViewModel : IDataErrorInfo
    {
        // Members
        private IValidate validator;
        private Dictionary<string, bool> validProperties = new Dictionary<string, bool>()
        {
            {"Language", false },
            {"Folder", false}
        };
        private bool allPropertiesValid = false;
        // Properties
        public bool AllPropertiesValid
        {
            get => allPropertiesValid;
            set => SetProperty(ref allPropertiesValid, value);
        }
        // Methods
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
    /// <summary>
    /// Part of the class that provides data for a View.
    /// </summary>
    public partial class LangWindowViewModel : BindableBase
    {
        //Members
        private IEventAggregator eventAggregator;
        private IUIMainWindowService windowService;
        private IDataProvider dataProvider;
        private ILoggerFacade logger;
        private string language;
        private string folder;

        // Properties
        public ObservableCollection<LingvaViewModel> Languages { get; }
        public LingvaViewModel SelectedLanguage { get; set; }
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
        public ICommand GetFolder { get; }
        public ICommand AddLanguage { get; }
        public ICommand RemoveLanguage { get; }
        // ctor
        public LangWindowViewModel(IUIMainWindowService windowService, IDataProvider dataProvider, IValidate validator,
            ILoggerFacade logger, IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.windowService = windowService;
            this.validator = validator;
            this.dataProvider = dataProvider;
            this.logger = logger;
            // Commands
            GetFolder = new DelegateCommand(_GetFolder);
            AddLanguage = new DelegateCommand(_AddLanguage);
            RemoveLanguage = new DelegateCommand(_RemoveLanguage);
            // Display initial list of languages
            Languages = new ObservableCollection<LingvaViewModel>();
            foreach (ILingva lang in dataProvider.GetLanguages())
            {
                Languages.Add(new LingvaViewModel(lang));
            }
        }
        // Methods
        private void _GetFolder()
        {
            if (windowService.SelectFolder(out string folderName))
            {
                logger.Log(string.Format("Selected new folder for language: {0}", folderName),
                    Category.Debug, Priority.Medium);
                Folder = folderName;
            }
        }
        private void _AddLanguage()
        {
            logger.Log("Adding new language.", Category.Debug, Priority.Medium);
            ILingva newLang = dataProvider.CreateLanguage(Language, Folder);
            LingvaViewModel lvm = new LingvaViewModel(newLang);
            Languages.Add(lvm);
            eventAggregator.GetEvent<LanguageAddedEvent>().Publish(lvm);
            // Clear text controls.
            Language = string.Empty;
            Folder = string.Empty;
        }
        private void _RemoveLanguage()
        {
            if (SelectedLanguage != null)
            {
                logger.Log(string.Format("Removing language: {0}.", SelectedLanguage.Language),
                    Category.Debug, Priority.Medium);
                ILingva lang = SelectedLanguage.Lingva;
                dataProvider.RemoveLanguage(lang);
                eventAggregator.GetEvent<LanguageDeletedEvent>().Publish(SelectedLanguage);
                Languages.Remove(SelectedLanguage);
            }
        }
    }
}
