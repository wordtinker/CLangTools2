using Models.Interfaces;

namespace ViewModels
{

    public class DictViewModel
    {
        private IDict dictionary;

        // Properties
        public string FileName
        {
            get => dictionary.FileName;
        }

        // TODO or DictType
        public string DictType
        {
            get => dictionary.DictType.ToString();
        }

        public string FilePath
        {
            get => dictionary.FilePath;
        }
        // Constructors
        public DictViewModel(IDict dictionary)
        {
            this.dictionary = dictionary;
        }
    }

    // TODO STUB
    ///// <summary>
    ///// Represents dictionary file.
    ///// </summary>
    //public class DictViewModel
    //{
    //    // Members
    //    private IUIBaseService windowService;

    //    // Constructors
    //    public DictViewModel(IUIBaseService windowService, Dict dictionary)
    //    {
    //        this.windowService = windowService;
    //        this.currentDictionary = dictionary;
    //    }

    //    // Methods
    //    public void OpenFile()
    //    {
    //        if (!IOTools.OpenWithDefault(FilePath))
    //        {
    //            windowService.ShowMessage(string.Format("Can't open {0}.", FilePath));
    //        }
    //    }

    //    public void DeleteFile()
    //    {
    //        if (windowService.Confirm(string.Format("Do you want to delete\n {0} ?", FilePath)))
    //        {
    //            if (!IOTools.DeleteFile(FilePath))
    //            {
    //                windowService.ShowMessage(string.Format("Can't delete {0}.", FilePath));
    //            }
    //        }
    //    }

    //    // Equals implementation
    //    public override bool Equals(object obj)
    //    {
    //        DictViewModel item = obj as DictViewModel;
    //        if (item == null)
    //        {
    //            return false;
    //        }

    //        return this.currentDictionary.Equals(item.currentDictionary);

    //    }

    //    public override int GetHashCode()
    //    {
    //        return currentDictionary.GetHashCode();
    //    }

    //}
}
