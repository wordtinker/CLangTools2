using Models.Interfaces;
using Prism.Mvvm;

namespace ViewModels
{
    /// <summary>
    /// Represents statistical data of the text file.
    /// </summary>
    public class FileStatsViewModel : BindableBase
    {
        private IFileStats fileStats;
        private bool highlighted;

        public IFileStats FileStats { get => fileStats; }
        public string FileName { get => fileStats.FileName; }
        public int? Size { get; private set; }
        public int? Known { get; private set; }
        public int? Unknown { get; private set; }
        public int? Maybe { get; private set; }
        public double? KnownPercent { get => ConvertToPercent(Known, Size); }
        public double? MaybePercent { get => ConvertToPercent(Maybe, Size); }
        public double? UnknownPercent { get => ConvertToPercent(Unknown, Size); }
        public bool Highlighted
        {
            get => highlighted;
            set => SetProperty(ref highlighted, value);
        }

        public FileStatsViewModel(IFileStats fileStats)
        {
            this.fileStats = fileStats;
            Update(fileStats);
        }
        public void Update(IFileStats fileStats)
        {
            Size = fileStats.Size;
            Known = fileStats.Known;
            Unknown = fileStats.Unknown;
            Maybe = fileStats.Maybe;
            // Notify that every Property changed
            RaisePropertyChanged(string.Empty);
        }
        // TODO refactor to Shared.Tools.Math
        private double? ConvertToPercent(int? dividend, int? divisor)
        {
            if (divisor == null || divisor == 0 || dividend == null)
            {
                return null;
            }

            return (double)dividend / divisor;
        }
        // Equals implementation
        // for .Intersect and deletion from Collections
        public override bool Equals(object obj)
        {
            if (obj is FileStatsViewModel item)
            {
                return this.fileStats.Equals(item.fileStats);
            }
            return false;
        }
        public override int GetHashCode()
        {
            return fileStats.GetHashCode();
        }
    }
    // TODO STUB
    //public class FileStatsViewModel : BindableBase
    //{
    //    private IUIBaseService windowService;


    //    public string FilePath { get { return fileStats.FilePath; } }
    //    public Lingva Lingva { get { return fileStats.Lingva; } }
    //    public string Project { get { return fileStats.Project; } }
    //    public string OutPath { get { return fileStats.OutPath; } }

    //    public FileStatsViewModel(IUIBaseService windowService, FileStats fileStats)
    //    {
    //        this.windowService = windowService;
    //        this.fileStats = fileStats;
    //        this.fileStats.PropertyChanged += (obj, e) =>
    //        {
    //            // Raise all properties changed
    //            RaisePropertyChanged(string.Empty);
    //        };
    //    }

    //    public void OpenOutput()
    //    {
    //        if (OutPath != null)
    //        {
    //            if (!IOTools.OpenWithDefault(OutPath))
    //            {
    //                windowService.ShowMessage(string.Format("Can't open {0}.", OutPath));
    //            }
    //        }
    //    }

    //    public void OpenFile()
    //    {
    //        if (!IOTools.OpenWithDefault(FilePath))
    //        {
    //            windowService.ShowMessage(string.Format("Can't open {0}.", FilePath));
    //        }
    //    }

    //    public void DeleteOutput()
    //    {
    //        if (OutPath != null && windowService.Confirm(string.Format("Do you want to delete\n {0} ?", OutPath)))
    //        {
    //            if (!IOTools.DeleteFile(OutPath))
    //            {
    //                windowService.ShowMessage(string.Format("Can't delete {0}.", OutPath));
    //            }
    //        }
    //    }

    //    public void DeleteFile()
    //    {
    //        // Model will be updated after FileWatcher catches the event
    //        if (windowService.Confirm(string.Format("Do you want to delete\n {0} ?", FilePath)))
    //        {
    //            if (!IOTools.DeleteFile(FilePath))
    //            {
    //                windowService.ShowMessage(string.Format("Can't delete {0}.", FilePath));
    //            }
    //        }
    //        // Delete output file together
    //        DeleteOutput();
    //    }
    //}
}
