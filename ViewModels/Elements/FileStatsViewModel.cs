using Models.Interfaces;
using Prism.Mvvm;
using Shared.Tools;

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
        public string FilePath { get => fileStats.FilePath; }
        public string OutPath { get; private set; }
        public int? Size { get; private set; }
        public int? Known { get; private set; }
        public int? Unknown { get; private set; }
        public int? Maybe { get; private set; }
        public double? KnownPercent { get => Math.TakePercent(Known, Size); }
        public double? MaybePercent { get => Math.TakePercent(Maybe, Size); }
        public double? UnknownPercent { get => Math.TakePercent(Unknown, Size); }
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
            OutPath = fileStats.OutPath;
            // Notify that every Property changed
            RaisePropertyChanged(string.Empty);
        }
        // Equals implementation
        // for .Intersect and deletion from Collections
        public override bool Equals(object obj)
        {
            if (obj is FileStatsViewModel other)
            {
                return this.fileStats.Equals(other.fileStats);
            }
            return false;
        }
        public override int GetHashCode()
        {
            return fileStats.GetHashCode();
        }
    }
}
